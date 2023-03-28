using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WatchStore.Models;

namespace WatchStore.Controllers
{
    
    public class GioHangController : Controller
    {
        // GET: GioHang
        dbDongHoDataContext db = new dbDongHoDataContext("Data Source=FREEDY\\SQLEXPRESS;Initial Catalog=DongHo;Integrated Security=True");
        [CustomAuthorlizeAttribute(Roles = "Khách Hàng")]
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        [CustomAuthorlizeAttribute(Roles = "Khách Hàng")]
        public ActionResult ThemGioHang(string id, string strURL)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang watch = lstGioHang.Find(n => n.IDWatch == id);
            if (watch == null)
            {
                watch = new GioHang(id);
                lstGioHang.Add(watch);
                return Redirect(strURL);
            }
            else
            {
                watch.iSoLuong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(n => n.iSoLuong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tt = lstGioHang.Sum(n => n.ThanhTien);
            }
            return tt;
        }
        [CustomAuthorlizeAttribute(Roles = "Khách Hàng")]
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }
        public ActionResult XoaGioHang(string id)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.IDWatch == id);
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.IDWatch == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(string id, FormCollection collection)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.IDWatch == id);
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(collection["txtSolg"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("GioHang");
        }
        public ActionResult Index()
        {
            return View();
        }
        /*Giohang k loi */
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Email"] == null || Session["Email"].ToString() == " ")
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Watch");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGioHang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            Bill dh = new Bill();
            Customer kh = (Customer)Session["Email"];
            Watch s = new Watch();
            List<GioHang> gh = LayGioHang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dh.IDCustomer = kh.IDCustomer;
            dh.Time = DateTime.Now;
            dh.Status = false;
            db.Bills.InsertOnSubmit(dh);
            db.SubmitChanges();
            foreach (var item in gh)
            {
                BillDetail ctdh = new BillDetail();
                ctdh.IDBill = dh.IDBill;
                ctdh.IDWatch = item.IDWatch;
                ctdh.NumberOfOrders = item.iSoLuong;
                /* ctdh.Gia = (decimal)item.Price;*/
                s = db.Watches.Single(n => n.IDWatch == item.IDWatch);

                db.SubmitChanges();
                db.BillDetails.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacnhanDonHang", "GioHang");
        }
        public ActionResult XacnhanDonhang()
        {
            return View();
        }
    }
}