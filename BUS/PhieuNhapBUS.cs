using DAL.Implementations;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class PhieuNhapBUS
    {
        private PhieuNhapDAL _pnDAL = new PhieuNhapDAL();

        public List<PhieuNhap> LayDS()
        {
            return _pnDAL.LayDS();
        }

        public bool LuuPhieuNhap(PhieuNhap pn)
        {
            return _pnDAL.LuuPhieuNhap(pn);
        }

        public bool NhapHangVaoKho(PhieuNhap pn, List<ChiTietPN> dsChiTiet)
        {
            // 1. Lưu phiếu nhập
            if (_pnDAL.LuuPhieuNhap(pn))
            {
                // 2. Lưu chi tiết phiếu nhập (tự động cộng tồn kho ở SQL)
                foreach (var item in dsChiTiet)
                {
                    _pnDAL.LuuChiTiet(item);
                }
                return true;
            }
            return false;
        }
    }
}
