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

        public bool LuuHoaDon(HoaDon hd)
        {
            return _hdDAL.LuuHoaDon(hd);
        }

        public bool XuatHoaDon(HoaDon hd, List<ChiTietHD> dsChiTiet)
        {
            // 1. Lưu thông tin hóa đơn tổng trước
            if (_hdDAL.LuuHoaDon(hd))
            {
                // 2. Lưu từng dòng chi tiết
                foreach (var item in dsChiTiet)
                {
                    _hdDAL.LuuChiTiet(item);
                }
                return true;
            }
            return false;
        }
    }
}
