using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   
        public class LoaiSanPham { public string MaLoai { get; set; } public string TenLoai { get; set; } }
        public class SanPham
        {
            public string MaSP { get; set; }
            public string TenSP { get; set; }
            public decimal GiaBan { get; set; }
            public int SoLuongTon { get; set; }
            public string DonViTinh { get; set; }
            public string MaLoai { get; set; }
        }
        public class KhachHang { public string MaKH { get; set; } public string TenKH { get; set; } public string SoDienThoai { get; set; } public string DiaChi { get; set; } }
        public class HoaDon { public string MaHD { get; set; } public DateTime NgayLap { get; set; } public string MaKH { get; set; } public decimal TongTien { get; set; } }
        public class ChiTietHD { public string MaHD { get; set; } public string MaSP { get; set; } public int SoLuong { get; set; } public decimal DonGia { get; set; } }
        public class PhieuNhap { public string MaPN { get; set; } public DateTime NgayNhap { get; set; } public decimal TongTienNhap { get; set; } }
        public class ChiTietPN { public string MaPN { get; set; } public string MaSP { get; set; } public int SoLuong { get; set; } public decimal GiaNhap { get; set; } }
}

