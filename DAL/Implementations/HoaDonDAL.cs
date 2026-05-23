using DAL.Helper;
using DAL.Interfaces;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class HoaDonDAL : IHoaDon
    {
        dbConnection db = new dbConnection();

        public List<HoaDon> LayDS()
        {
            DataTable dt = db.ExecuteQueryText("SELECT MaHD, NgayLap, MaKH, TongTien FROM HoaDon");
            List<HoaDon> list = new List<HoaDon>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new HoaDon
                {
                    MaHD = r["MaHD"].ToString(),
                    NgayLap = Convert.ToDateTime(r["NgayLap"]),
                    MaKH = r["MaKH"].ToString(),
                    TongTien = Convert.ToDecimal(r["TongTien"])
                });
            }
            return list;
        }

        public bool LuuHoaDon(HoaDon hd)
        {
            MySqlParameter[] p = {
                new MySqlParameter("p_MaHD", hd.MaHD),
                new MySqlParameter("p_Ngay", hd.NgayLap),
                new MySqlParameter("p_MaKH", hd.MaKH),
                new MySqlParameter("p_Tong", hd.TongTien)
            };
            return db.ExecuteNonQuery("sp_LuuHoaDon", p);
        }

        public bool LuuChiTiet(ChiTietHD ct)
        {
            MySqlParameter[] p = {
                new MySqlParameter("p_MaHD", ct.MaHD),
                new MySqlParameter("p_MaSP", ct.MaSP),
                new MySqlParameter("p_SoLuong", ct.SoLuong),
                new MySqlParameter("p_DonGia", ct.DonGia)
            };
            return db.ExecuteNonQuery("sp_LuuChiTietHD", p);
        }

        public string LayMaHDMoi()
        {
            // Logic đơn giản: Lấy Count + 1, bạn có thể cải tiến tùy ý
            DataTable dt = db.ExecuteQueryText("SELECT COUNT(*) AS TotalCount FROM HoaDon");
            int count = Convert.ToInt32(dt.Rows[0]["TotalCount"]) + 1;
            return "HD" + count.ToString("D3");
        }

        public List<ChiTietHD> LayTatCaChiTiet()
        {
            DataTable dt = db.ExecuteQueryText("SELECT MaHD, MaSP, SoLuong, DonGia FROM ChiTietHD");
            List<ChiTietHD> list = new List<ChiTietHD>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new ChiTietHD
                {
                    MaHD = r["MaHD"].ToString(),
                    MaSP = r["MaSP"].ToString(),
                    SoLuong = Convert.ToInt32(r["SoLuong"]),
                    DonGia = Convert.ToDecimal(r["DonGia"])
                });
            }
            return list;
        }

        // Lưu hóa đơn + toàn bộ chi tiết trong 1 transaction. Mã HĐ được sinh
        // bên trong transaction (MAX+1) để không race với phiên khác.
        // Trên hd và mỗi item của dsChiTiet, MaHD sẽ được set lại sau khi sinh.
        public void XuatHoaDonAtomic(HoaDon hd, List<ChiTietHD> dsChiTiet)
        {
            db.ExecuteTransaction((conn, tx) =>
            {
                // Sinh MaHD race-free trong transaction
                using (var cmdGen = new MySqlCommand(
                    "SELECT COALESCE(MAX(CAST(SUBSTRING(MaHD,3) AS UNSIGNED)),0)+1 FROM HoaDon FOR UPDATE",
                    conn, tx))
                {
                    int next = Convert.ToInt32(cmdGen.ExecuteScalar());
                    hd.MaHD = "HD" + next.ToString("D3");
                }
                foreach (var ct in dsChiTiet) ct.MaHD = hd.MaHD;

                // INSERT HoaDon (MaKH rỗng -> NULL để giữ schema sạch)
                using (var cmdHD = new MySqlCommand("sp_LuuHoaDon", conn, tx) { CommandType = CommandType.StoredProcedure })
                {
                    cmdHD.Parameters.AddWithValue("p_MaHD", hd.MaHD);
                    cmdHD.Parameters.AddWithValue("p_Ngay", hd.NgayLap);
                    cmdHD.Parameters.AddWithValue("p_MaKH", string.IsNullOrWhiteSpace(hd.MaKH) ? (object)DBNull.Value : hd.MaKH);
                    cmdHD.Parameters.AddWithValue("p_Tong", hd.TongTien);
                    cmdHD.ExecuteNonQuery();
                }

                // INSERT từng ChiTietHD (SP tự trừ tồn kho)
                foreach (var ct in dsChiTiet)
                {
                    using (var cmdCT = new MySqlCommand("sp_LuuChiTietHD", conn, tx) { CommandType = CommandType.StoredProcedure })
                    {
                        cmdCT.Parameters.AddWithValue("p_MaHD", ct.MaHD);
                        cmdCT.Parameters.AddWithValue("p_MaSP", ct.MaSP);
                        cmdCT.Parameters.AddWithValue("p_SoLuong", ct.SoLuong);
                        cmdCT.Parameters.AddWithValue("p_DonGia", ct.DonGia);
                        cmdCT.ExecuteNonQuery();
                    }
                }
            });
        }
    }
}
