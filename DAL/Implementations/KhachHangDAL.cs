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
    public class KhachHangDAL : IKhachHang
    {
        dbConnection db = new dbConnection();

        public List<KhachHang> LayDS()
        {
            DataTable dt = db.ExecuteQueryText("SELECT MaKH, TenKH, SoDienThoai, DiaChi FROM KhachHang");
            List<KhachHang> list = new List<KhachHang>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new KhachHang
                {
                    MaKH = r["MaKH"].ToString(),
                    TenKH = r["TenKH"].ToString(),
                    SoDienThoai = r["SoDienThoai"].ToString(),
                    DiaChi = r["DiaChi"].ToString()
                });
            }
            return list;
        }

        public KhachHang TimTheoSDT(string sdt)
        {
            MySqlParameter[] p = { new MySqlParameter("p_sdt", sdt) };
            DataTable dt = db.ExecuteQueryText("SELECT MaKH, TenKH, SoDienThoai, DiaChi FROM KhachHang WHERE SoDienThoai = @p_sdt LIMIT 1", p);
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                return new KhachHang
                {
                    MaKH = r["MaKH"].ToString(),
                    TenKH = r["TenKH"].ToString(),
                    SoDienThoai = r["SoDienThoai"].ToString(),
                    DiaChi = r["DiaChi"].ToString()
                };
            }
            return null;
        }

        public bool Them(KhachHang kh)
        {
            MySqlParameter[] p = {
                new MySqlParameter("p_MaKH", kh.MaKH),
                new MySqlParameter("p_TenKH", kh.TenKH),
                new MySqlParameter("p_SDT", kh.SoDienThoai),
                new MySqlParameter("p_DiaChi", kh.DiaChi)
            };
            return db.ExecuteNonQuery("sp_ThemKhachHang", p);
        }

        public bool Sua(KhachHang kh)
        {
            MySqlParameter[] p = {
                new MySqlParameter("p_MaKH", kh.MaKH),
                new MySqlParameter("p_TenKH", kh.TenKH),
                new MySqlParameter("p_SDT", kh.SoDienThoai),
                new MySqlParameter("p_DiaChi", kh.DiaChi)
            };
            return db.ExecuteNonQuery("sp_SuaKhachHang", p);
        }

        public bool Xoa(string maKH)
        {
            MySqlParameter[] p = {
                new MySqlParameter("p_MaKH", maKH)
            };
            return db.ExecuteNonQueryText("DELETE FROM KhachHang WHERE MaKH = @p_MaKH", p);
        }
    }
}
