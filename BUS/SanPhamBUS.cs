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

        public bool LuuSanPham(SanPham sp)
        {
            // Nghiệp vụ: Giá bán không được âm
            if (sp.GiaBan < 0) return false;

            return _spDAL.Them(sp);
        }

        public bool SuaSanPham(SanPham sp)
        {
            if (sp.GiaBan < 0) return false;
            return _spDAL.Sua(sp);
        }

        public bool XoaSanPham(string maSP)
        {
            return _spDAL.Xoa(maSP);
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
