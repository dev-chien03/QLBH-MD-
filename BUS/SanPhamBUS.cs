using DAL;
using DAL.Implementations;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class SanPhamBUS
    {
        private SanPhamDAL _spDAL = new SanPhamDAL();

        public List<SanPham> LayDS()
        {
            return _spDAL.LayDS();
        }

        public bool ThemSanPham(SanPham sp)
        {
            // Nghiệp vụ: Kiểm tra dữ liệu hợp lệ
            if (string.IsNullOrWhiteSpace(sp.MaSP) || string.IsNullOrWhiteSpace(sp.TenSP))
                return false;
            
            if (sp.GiaBan < 0 || sp.SoLuongTon < 0)
                return false;

            return _spDAL.Them(sp);
        }

        public bool SuaSanPham(SanPham sp)
        {
            if (string.IsNullOrWhiteSpace(sp.MaSP) || string.IsNullOrWhiteSpace(sp.TenSP))
                return false;
            
            if (sp.GiaBan < 0 || sp.SoLuongTon < 0)
                return false;
            
            return _spDAL.Sua(sp);
        }

        public bool XoaSanPham(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP))
                return false;
            
            try
            {
                return _spDAL.Xoa(maSP);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in XoaSanPham: {ex.Message}");
                return false;
            }
        }
    }

    public class LoaiSPBUS
    {
        private LoaiSPDAL _loaiDAL = new LoaiSPDAL();
        public List<LoaiSanPham> LayDS()
        {
            return _loaiDAL.LayDS();
        }
    }
}
