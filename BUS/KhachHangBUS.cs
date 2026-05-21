using DAL.Implementations;

using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BUS
{
    public class KhachHangBUS
    {
        // Sử dụng Interface để gọi tầng DAL
        private KhachHangDAL _khDAL = new KhachHangDAL();

        public List<KhachHang> LayDS()
        {
            return _khDAL.LayDS();
        }

        public bool ThemKhachHang(KhachHang kh)
        {
            // Nghiệp vụ: Tên và SĐT không được để trống
            if (string.IsNullOrWhiteSpace(kh.TenKH) || string.IsNullOrWhiteSpace(kh.SoDienThoai))
                return false;
            return _khDAL.Them(kh);
        }

        public bool SuaKhachHang(KhachHang kh)
        {
            return _khDAL.Sua(kh);
        }

        public bool XoaKhachHang(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                return false;

            return _khDAL.Xoa(maKH);
        }

        // Logic "Tự động điền" khi người dùng nhập SĐT ở GUI
        public KhachHang TimKiemNhanh(string sdt)
        {
            if (string.IsNullOrEmpty(sdt)) return null;
            return _khDAL.TimTheoSDT(sdt);
        }
    }
}
