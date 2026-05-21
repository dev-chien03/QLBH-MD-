using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL.Interfaces
{
    internal interface IHoaDon
    {
        bool LuuHoaDon(HoaDon hd);
        bool LuuChiTiet(ChiTietHD ct);
        string LayMaHDMoi();
    }
}
