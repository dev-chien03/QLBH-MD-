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
    public class PhieuNhapDAL : IPhieuNhap
    {
        dbConnection db = new dbConnection();

        public List<PhieuNhap> LayDS()
        {
            DataTable dt = db.ExecuteQueryText("SELECT MaPN, NgayNhap, TongTienNhap FROM PhieuNhap");
            List<PhieuNhap> list = new List<PhieuNhap>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new PhieuNhap
                {
                    MaPN = r["MaPN"].ToString(),
                    NgayNhap = Convert.ToDateTime(r["NgayNhap"]),
                    TongTienNhap = Convert.ToDecimal(r["TongTienNhap"])
                });
            }
            return list;
        }

        public bool LuuPhieuNhap(PhieuNhap pn)
        {
            MySqlParameter[] p = {
                new MySqlParameter("p_MaPN", pn.MaPN),
                new MySqlParameter("p_Ngay", pn.NgayNhap),
                new MySqlParameter("p_Tong", pn.TongTienNhap)
            };
            return db.ExecuteNonQuery("sp_LuuPhieuNhap", p);
        }

        public bool LuuChiTiet(ChiTietPN ct)
        {
            MySqlParameter[] p = {
                new MySqlParameter("p_MaPN", ct.MaPN),
                new MySqlParameter("p_MaSP", ct.MaSP),
                new MySqlParameter("p_SoLuong", ct.SoLuong),
                new MySqlParameter("p_GiaNhap", ct.GiaNhap)
            };
            return db.ExecuteNonQuery("sp_LuuChiTietPN", p);
        }

        public string LayMaPNMoi()
        {
            DataTable dt = db.ExecuteQueryText("SELECT COUNT(*) AS TotalCount FROM PhieuNhap");
            int count = Convert.ToInt32(dt.Rows[0]["TotalCount"]) + 1;
            return "PN" + count.ToString("D3");
        }

        public List<ChiTietPN> LayTatCaChiTiet()
        {
            DataTable dt = db.ExecuteQueryText("SELECT MaPN, MaSP, SoLuong, GiaNhap FROM ChiTietPN");
            List<ChiTietPN> list = new List<ChiTietPN>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new ChiTietPN
                {
                    MaPN = r["MaPN"].ToString(),
                    MaSP = r["MaSP"].ToString(),
                    SoLuong = Convert.ToInt32(r["SoLuong"]),
                    GiaNhap = Convert.ToDecimal(r["GiaNhap"])
                });
            }
            return list;
        }

        // Lưu phiếu nhập + toàn bộ chi tiết trong 1 transaction. Mã PN sinh trong tx.
        public void NhapKhoAtomic(PhieuNhap pn, List<ChiTietPN> dsChiTiet)
        {
            db.ExecuteTransaction((conn, tx) =>
            {
                using (var cmdGen = new MySqlCommand(
                    "SELECT COALESCE(MAX(CAST(SUBSTRING(MaPN,3) AS UNSIGNED)),0)+1 FROM PhieuNhap FOR UPDATE",
                    conn, tx))
                {
                    int next = Convert.ToInt32(cmdGen.ExecuteScalar());
                    pn.MaPN = "PN" + next.ToString("D3");
                }
                foreach (var ct in dsChiTiet) ct.MaPN = pn.MaPN;

                using (var cmdPN = new MySqlCommand("sp_LuuPhieuNhap", conn, tx) { CommandType = CommandType.StoredProcedure })
                {
                    cmdPN.Parameters.AddWithValue("p_MaPN", pn.MaPN);
                    cmdPN.Parameters.AddWithValue("p_Ngay", pn.NgayNhap);
                    cmdPN.Parameters.AddWithValue("p_Tong", pn.TongTienNhap);
                    cmdPN.ExecuteNonQuery();
                }

                foreach (var ct in dsChiTiet)
                {
                    using (var cmdCT = new MySqlCommand("sp_LuuChiTietPN", conn, tx) { CommandType = CommandType.StoredProcedure })
                    {
                        cmdCT.Parameters.AddWithValue("p_MaPN", ct.MaPN);
                        cmdCT.Parameters.AddWithValue("p_MaSP", ct.MaSP);
                        cmdCT.Parameters.AddWithValue("p_SoLuong", ct.SoLuong);
                        cmdCT.Parameters.AddWithValue("p_GiaNhap", ct.GiaNhap);
                        cmdCT.ExecuteNonQuery();
                    }
                }
            });
        }
    }
}
