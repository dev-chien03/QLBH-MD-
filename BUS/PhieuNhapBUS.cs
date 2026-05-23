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
            // Lưu nguyên tử: PhieuNhap + tất cả ChiTietPN trong 1 transaction.
            // MaPN sinh trong transaction (MAX+1), gán lại vào pn.MaPN.
            _pnDAL.NhapKhoAtomic(pn, dsChiTiet);
            return true;
        }

        public string TaoMaPNMoi()
        {
            return _pnDAL.LayMaPNMoi();
        }

        public List<ChiTietPN> LayTatCaChiTiet()
        {
            return _pnDAL.LayTatCaChiTiet();
        }
    }
}
