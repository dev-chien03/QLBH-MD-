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
            try
            {
                MySqlParameter[] p = {
                    new MySqlParameter("p_MaSP", sp.MaSP), new MySqlParameter("p_TenSP", sp.TenSP),
                    new MySqlParameter("p_Gia", sp.GiaBan), new MySqlParameter("p_Ton", sp.SoLuongTon),
                    new MySqlParameter("p_DVT", sp.DonViTinh), new MySqlParameter("p_MaLoai", sp.MaLoai)
                };
                db.ExecuteNonQuery("sp_SuaSanPham", p);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating product: {ex.Message}");
                return false;
            }
        }

        public bool Xoa(string maSP)
        {
            try
            {
                // Kiểm tra sản phẩm có được sử dụng trong ChiTietHD không
                DataTable dtHD = db.ExecuteQueryText($"SELECT COUNT(*) as cnt FROM ChiTietHD WHERE MaSP='{maSP}'");
                if (dtHD != null && dtHD.Rows.Count > 0 && Convert.ToInt32(dtHD.Rows[0]["cnt"]) > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Cannot delete product {maSP}: Used in sales invoices");
                    return false;
                }

                // Kiểm tra sản phẩm có được sử dụng trong ChiTietPN không
                DataTable dtPN = db.ExecuteQueryText($"SELECT COUNT(*) as cnt FROM ChiTietPN WHERE MaSP='{maSP}'");
                if (dtPN != null && dtPN.Rows.Count > 0 && Convert.ToInt32(dtPN.Rows[0]["cnt"]) > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Cannot delete product {maSP}: Used in import notes");
                    return false;
                }

                // Nếu không được sử dụng, tiến hành xóa bằng SQL trực tiếp
                // (Không gọi sp_XoaSanPham vì SP có câu SELECT cuối làm ExecuteNonQuery trả về -1)
                MySqlParameter[] p = { new MySqlParameter("@p_maSP", maSP) };
                return db.ExecuteNonQueryText("DELETE FROM SanPham WHERE MaSP COLLATE utf8mb4_unicode_ci = @p_maSP COLLATE utf8mb4_unicode_ci", p);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting product: {ex.Message}");
                return false;
            }
        }
    }

}
