using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    internal interface ISanPham
    {
        List<SanPham> LayDS();
        bool Them(SanPham sp);
        bool Sua(SanPham sp);
        bool Xoa(string maSP);
    }
}
