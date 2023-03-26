using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;

namespace WatchStore.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        dbDongHoDataContext db=new dbDongHoDataContext("Data Source=DESKTOP-NEIOBVT;Initial Catalog=WatchStore;Integrated Security=True");
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TaoDongHo()
        {
            List<Supplier> listS = db.Suppliers.ToList();
            ViewBag.Supplier1 = new SelectList(listS, "IDSupplier", "NameSupplier");
            List<Brand> listB = db.Brands.ToList();
            ViewBag.Brand = new SelectList(listB, "IDBrand", "NameBrand");
            List<Origin> listO = db.Origins.ToList();
            ViewBag.Origin = new SelectList(listO, "IDOrigin", "NameOrigin");
            List<ProductFor> listP = db.ProductFors.ToList();
            ViewBag.ProductFor = new SelectList(listP, "IDProductFor", "NameProductFor");
            return View();
        }
        [HttpPost]
        public ActionResult TaoDongHo(FormCollection collection, Watch w)
        {
            var IDWatch = from b in db.AUTO_IDWatch() select b;
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
                foreach (var item in IDWatch)
                {
                    w.IDWatch += item;
                }
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
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
    }
}