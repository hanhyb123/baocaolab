using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_BookStore_Toan.Models;


namespace web_BookStore_Toan.Controllers
{
    public class BookStoreController : Controller
    {
        QLBanSachEntities1 db = new QLBanSachEntities1();

        // GET: BookStore
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sachmoi()
        {
            var lstsach = db.SACHes.ToList();
            return PartialView(lstsach);
        }
        public PartialViewResult ChuDePartial()
        {
            var lstchude = from cd in db.CHUDEs select cd;
            return PartialView(lstchude);
        }
        public PartialViewResult NhaXuatBanPartial()
        {
            var lstnxb = db.NHAXUATBANs.Take(5).ToList();
            return PartialView(lstnxb);
        }
        public PartialViewResult SanPhamPartial()
        {
            var lstsach = db.SACHes.Take(5).ToList();
            return PartialView(lstsach);
        }
        public ViewResult XemChiTiet(int Masach)
        {
            SACH SACH = db.SACHes.SingleOrDefault(n => n.Masach == Masach);
            //SingleOrDefaul giúp trả về đối tượng
            if (SACH == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(SACH);
        }
        public PartialViewResult SanPhamTrend()
        {
            var lstsach = db.SACHes.Take(3).ToList();
            return PartialView(lstsach);
        }
        public ViewResult XemChiTietsp(int Masach)
        {
            var sach = from s in db.SACHes where s.Masach == Masach select s;
            return View(sach.Single());
        }
        public ActionResult SPTheochude(int Macd)
        {
            var sach = from s in db.SACHes where s.Masach == Macd select s;
            return PartialView(sach);
        }
        public ActionResult SPTheoNxb(int NXB)
        {
            var Sach = from s in db.SACHes where s.MaNXB == NXB select s;
            return View(Sach);
        }
        public PartialViewResult SanPhamnoibat()
        {
            var lstsach = db.SACHes.Take(5).ToList();
            return PartialView(lstsach);
        }
        public PartialViewResult SanPhamtotnhat()
        {
            var lstsach = db.SACHes.Take(5).ToList();
            return PartialView(lstsach);
        }
        public PartialViewResult SanPhamdacsac()
        {
            var lstsach = db.SACHes.Take(5).ToList();
            return PartialView(lstsach);
        }
    }
}