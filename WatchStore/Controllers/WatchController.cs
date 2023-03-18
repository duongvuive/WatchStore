using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;
using PagedList;
using System.Drawing.Printing;
using System.Web.UI;

namespace WatchStore.Controllers
{
    public class WatchController : Controller
    {
        // GET: Watch
        dbDongHoDataContext db = new dbDongHoDataContext("Data Source=FREEDY\\SQLEXPRESS;Initial Catalog=hi;Integrated Security=True");
        int Nam = 1;//ID IDProductFor Nam
        int Nu = 2;//ID IDProductFor Nu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DongHoNam(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 6;
            }
            var N = from s in db.Watches.ToList() where s.IDProductFor == Nam select s;
            return View(N.ToPagedList((int)page, (int)pageSize));
        }

        public ActionResult DongHoNu(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 6;
            }
            var N = from s in db.Watches.ToList() where s.IDProductFor == Nu select s;
            ViewBag.Suppliers = db.Suppliers;
            return View(N.ToPagedList((int)page, (int)pageSize));
        }

        public ActionResult TaoDongHo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TaoDongHo(FormCollection collection, Watch w, string iDWatch)
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
        public ActionResult Detail(string id)
        {
            var D_dongho = db.Watches.FirstOrDefault(m => m.IDWatch == id);
            return View(D_dongho);
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
        public ActionResult Edit(string iDWatch)
        {
            var E_dongho = db.Watches.FirstOrDefault(m => m.IDWatch == iDWatch);
            return View(E_dongho);
        }
        [HttpPost]
        public ActionResult Edit(Watch w, string iDWatch, FormCollection collection)
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
            if (string.IsNullOrEmpty(NameWatch))
            {
                ViewData["Error"] = "Don't empty!";
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
                UpdateModel(w);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(iDWatch);
        }
    }
}