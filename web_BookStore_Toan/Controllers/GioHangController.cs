using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_BookStore_Toan.Models;

namespace web_BookStore_Toan.Controllers
{
    
    public class GioHangController : Controller
    {
        QLBanSachEntities1 db = new QLBanSachEntities1();
        // GET: GioHang
        // lấy giỏ hàng
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;

            }
            return lstGioHang;
        }
        // thêm vào giỏ hàng
        public ActionResult ThemGioHang(int iMasach, String strURL)
        {
            //Lấy sesion giỏ hàng
            List<GioHang> lstGioHang = Laygiohang();
            //ktra trong sách này có tồn tại trong giỏ hàng chưa
            GioHang sanpham = lstGioHang.Find(n => n.iMasach == iMasach);
            if (sanpham == null)
            {
                sanpham = new GioHang(iMasach);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);

            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoluong);

            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongTien = lstGioHang.Sum(n => n.dThanhtien);

            }
            return iTongTien;
        }
        //pthuc hiển thị giỏ hàng
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        //Tạo partial view để xem giỏ hànhg
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult Xoagiohang(int iMaSP)
        {
            //lấy giỏ hàng từ sesiom
            List<GioHang> lstGioHang = Laygiohang();
            //ktra xem sp có trong sesiom giỏ hàng chauw
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMasach == iMaSP);
            //nếu tồn tại thì cho sửa số lượng
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.iMasach == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int iMaSP, FormCollection f)
        {
            // lấy giỏ hàng từ sesiom
            List<GioHang> lstGioHang = Laygiohang();
            //ktra 
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMasach == iMaSP);
            //nêu tồn tại thí sửa số lượng
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");


        }

        public ActionResult XoaTatCaGioHang()
        {
            // lấy giỏ hàng
            List<GioHang> lstGioHang = Laygiohang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "BookStore");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            //ktra đăng nhâp
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("GioHang", "GioHang");
            }
            // lấy giỏ hàng tư sesion

            List<GioHang> lstGioHang = Laygiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            //Thêm đơn hàng
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<GioHang> GioHang = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH = DateTime.Now;
            var Ngaygiao = String.Format("{0:MM/dd/yyyy }", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(Ngaygiao);
            ddh.Trinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();
            //Theem chi tiet don hang
            foreach (var item in GioHang)
            {
                CTDATHANG ctdh = new CTDATHANG();
                ctdh.SoDH = ddh.SoDH;
                ctdh.SoDH = item.iMasach;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                db.CTDATHANGs.Add(ctdh);
            }
            Session["GioHang"] = null;
            db.SaveChanges();
            return RedirectToAction("Xacnhandonhang", "GioHang");



        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
        public ActionResult Thongtindathang()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}