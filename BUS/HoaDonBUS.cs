using DAL.Implementations;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class HoaDonBUS
    {
        private HoaDonDAL _hdDAL = new HoaDonDAL();

        public string TaoMaHDMoi()
        {
            return _hdDAL.LayMaHDMoi();
        }

        public List<HoaDon> LayDS()
        {
            return _hdDAL.LayDS();
        }

        public List<ChiTietHD> LayTatCaChiTiet()
        {
            return _hdDAL.LayTatCaChiTiet();
        }

        public bool LuuHoaDon(HoaDon hd)
        {
            return _hdDAL.LuuHoaDon(hd);
        }

        public bool XuatHoaDon(HoaDon hd, List<ChiTietHD> dsChiTiet)
        {
            // Lưu nguyên tử: HoaDon + tất cả ChiTietHD trong 1 transaction.
            // MaHD được sinh bên trong transaction (MAX+1) và gán lại vào hd.MaHD.
            _hdDAL.XuatHoaDonAtomic(hd, dsChiTiet);
            return true;
        }
    }
}
