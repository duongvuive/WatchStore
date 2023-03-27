using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;
using PagedList;
using System.Drawing.Printing;
using System.Web.UI;
using System.Web.Routing;

namespace WatchStore.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        dbDongHoDataContext db = new dbDongHoDataContext("Data Source=FREEDY\\SQLEXPRESS;Initial Catalog=hi;Integrated Security=True");
        int Nam = 1;//ID IDProductFor Nam
        int Nu = 2;//ID IDProductFor Nu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DHNam(string OrderBy)
        {
            var N = from s in db.Watches.ToList() where s.IDProductFor == Nam select s;
            switch (OrderBy)
            {
                case "Price_asc":
                    N = N.OrderBy(s => s.Price); break;
                case "Price-desc":
                    N = N.OrderByDescending(s => s.Price); break;
                case "Name_asc":
                    N = N.OrderBy(s => s.NameWatch); break;
                case "Name-desc":
                    N = N.OrderByDescending(s => s.NameWatch); break;
                default:
                    N = N.OrderBy(s => s.IDWatch); break;
            }
            ViewBag.Suppliers = db.Suppliers;
            return View(N);
        }

        public ActionResult DHNu(string OrderBy)
        {
            var N = from s in db.Watches.ToList() where s.IDProductFor == Nu select s;
            switch (OrderBy)
            {
                case "Price_asc":
                    N = N.OrderBy(s => s.Price); break;
                case "Price-desc":
                    N = N.OrderByDescending(s => s.Price); break;
                case "Name_asc":
                    N = N.OrderBy(s => s.NameWatch); break;
                case "Name-desc":
                    N = N.OrderByDescending(s => s.NameWatch); break;
                default:
                    N = N.OrderBy(s => s.IDWatch); break;
            }
            ViewBag.Suppliers = db.Suppliers;
            return View(N);
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
        public ActionResult Delete(string id)
        {
            var D_dongho = db.Watches.FirstOrDefault(m => m.IDWatch == id);
            return View(D_dongho);
        }
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            var D_dongho = db.Watches.Where(m => m.IDWatch == id).First();
            db.Watches.DeleteOnSubmit(D_dongho);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string id)
        {
            var E_dongho = db.Watches.First(m => m.IDWatch == id);
            return View(E_dongho);
        }
        [HttpPost]
        public ActionResult Edit(Watch w, string id, FormCollection collection)
        {
            var IDWatch = db.Watches.First(m => m.IDWatch == id);
            var NameWatch = collection["NameWatch"];
            var IDSupplier = collection["IDSupplier"];
            var IDBrand = collection["IDBrand"];
            var IDOrigin = collection["IDOrigin"];
            var Image = collection["Image"];
            var Price = collection["Price"];
            var Status = collection["Status"];
            var IDProductFor = collection["IDProductFor"];
            var Content = collection["Content"];
            if (string.IsNullOrEmpty(NameWatch))
            {
                ViewData["Error"] = "Don't empty!";
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
                UpdateModel(w);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }
    }
}