using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_BookStore_Toan.Models
{
    public class GioHang
    {
        QLBanSachEntities1 db = new QLBanSachEntities1();
        public int iMasach { set; get; }
        public string sTensach { set; get; }
        public string sAnhbia { set; get; }
        public double dDongia { set; get; }
        public int iSoluong { set; get; }
        public double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        public GioHang(int Masach)
        {
            iMasach = Masach;
            SACH sach = db.SACHes.Single(n => n.Masach == iMasach);
            sTensach = sach.Tensach;
            sAnhbia = sach.Anhbia;
            dDongia = double.Parse(sach.Giaban.ToString());
            iSoluong = 1;
        }
    }
}