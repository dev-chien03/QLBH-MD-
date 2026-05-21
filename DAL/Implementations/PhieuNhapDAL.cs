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
    }
}
