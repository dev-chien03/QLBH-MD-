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
    }
}
