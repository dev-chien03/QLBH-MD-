using DAL.Helper;
using DAL.Interfaces;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class LoaiSPDAL : ILoaiSP
    {
        dbConnection db = new dbConnection();
        public List<LoaiSanPham> LayDS()
        {
            DataTable dt = db.ExecuteQuery("sp_LayDSLoaiSP");
            List<LoaiSanPham> list = new List<LoaiSanPham>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new LoaiSanPham { MaLoai = r["MaLoai"].ToString(), TenLoai = r["TenLoai"].ToString() });
            }
            return list;
        }
    }
}
