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
    public class SanPhamDAL : ISanPham
    {
        dbConnection db = new dbConnection();

        public List<SanPham> LayDS()
        {
            try
            {
                DataTable dt = db.ExecuteQueryText("SELECT MaSP, TenSP, GiaBan, SoLuongTon, DonViTinh, MaLoai FROM SanPham");
                List<SanPham> list = new List<SanPham>();

                if (dt == null || dt.Rows.Count == 0)
                {
                    return list; // Return empty list if no data
                }

                foreach (DataRow r in dt.Rows)
                {
                    list.Add(new SanPham
                    {
                        MaSP = r["MaSP"].ToString(),
                        TenSP = r["TenSP"].ToString(),
                        GiaBan = Convert.ToDecimal(r["GiaBan"]),
                        SoLuongTon = Convert.ToInt32(r["SoLuongTon"]),
                        DonViTinh = r["DonViTinh"].ToString(),
                        MaLoai = r["MaLoai"].ToString()
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading products: {ex.Message}");
                return new List<SanPham>(); // Return empty list on error
            }
        }

        public bool Them(SanPham sp)
        {
            MySqlParameter[] p = {
                new MySqlParameter("p_MaSP", sp.MaSP), new MySqlParameter("p_TenSP", sp.TenSP),
                new MySqlParameter("p_Gia", sp.GiaBan), new MySqlParameter("p_Ton", sp.SoLuongTon),
                new MySqlParameter("p_DVT", sp.DonViTinh), new MySqlParameter("p_MaLoai", sp.MaLoai)
            };
            return db.ExecuteNonQuery("sp_ThemSanPham", p);
        }

        public bool Sua(SanPham sp)
        {
            MySqlParameter[] p = {
                new MySqlParameter("p_MaSP", sp.MaSP), new MySqlParameter("p_TenSP", sp.TenSP),
                new MySqlParameter("p_Gia", sp.GiaBan), new MySqlParameter("p_Ton", sp.SoLuongTon),
                new MySqlParameter("p_DVT", sp.DonViTinh), new MySqlParameter("p_MaLoai", sp.MaLoai)
            };
            return db.ExecuteNonQuery("sp_SuaSanPham", p);
        }

        public bool Xoa(string maSP)
        {
            MySqlParameter[] p = { new MySqlParameter("p_maSP", maSP) };
            return db.ExecuteNonQuery("sp_XoaSanPham", p);
        }
    }

}
