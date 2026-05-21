using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    internal interface IKhachHang
    {
        List<KhachHang> LayDS();
        bool Them(KhachHang kh);
        bool Sua(KhachHang kh);
        bool Xoa(string maKH);
        // Phương thức quan trọng nhất cho yêu cầu của bạn
        KhachHang TimTheoSDT(string sdt);
    }
}
