using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;

namespace WatchStore.Controllers
{
    public class WatchController : Controller
    {
        // GET: Watch
        dbDongHoDataContext db = new dbDongHoDataContext("Data Source=DESKTOP-NEIOBVT;Initial Catalog=WatchStore;Integrated Security=True");
        int Nam = 1;//ID IDProductFor Nam
        int Nu = 2;//ID IDProductFor Nu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DongHoNam()
        {
            var N = from s in db.Watches where s.IDProductFor == Nam select s;
            return View(N);
        }

        public ActionResult DongHoNu()
        {
            var N = from s in db.Watches where s.IDProductFor == Nu select s;
            ViewBag.Suppliers = db.Suppliers;
            return View(N);
        }

        public ActionResult TaoDongHo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TaoDongHo(FormCollection collection, Watch w)
        {
            var NameWatch = collection["NameWatch"];
            var IDSupplier = collection["IDSupplier"];
            var IDBrand = collection["IDBrand"];
            var IDOrigin = collection["IDOrigin"];
            var Image = collection["Image"];
            var Price = collection["Price"];
            var Status = collection["Status"];
            var IDProductFor = collection["IDProductFor"];
            var Content = collection["Content"];
            if (String.IsNullOrEmpty(IDProductFor))
            {
                ViewData["IDProductFor"] = "Danh sách IDProductFor chưa tồn tại !";
            }
            else
            {
                w.NameWatch = NameWatch;
                w.IDSupplier = Int32.Parse(IDSupplier);
                w.IDBrand = Int32.Parse(IDBrand);
                w.IDOrigin = Int32.Parse(IDOrigin);
                w.Image = Image;
                w.Price = decimal.Parse(Price);
                w.Status = bool.Parse(Status);
                w.IDProductFor = Int32.Parse(IDProductFor);
                w.Content = Content;
                db.Watches.InsertOnSubmit(w);
                db.SubmitChanges();
                return RedirectToAction("Index");

            }
            return this.TaoDongHo();
        }
    }
}