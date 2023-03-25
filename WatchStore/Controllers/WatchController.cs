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

namespace WatchStore.Controllers
{

    public class CustomAuthorlizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)//Nếu người dùng chưa đăng nhập chuyển trang sang Login
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    (new { controller = "Account", action = "Login", returnUrl = filterContext.HttpContext.Request.Url }));

            }
            //Nếu người dùng đăng nhập không có quyền truy cập 
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Loi", action = "Index" }));
            }
        }
    }
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

        [CustomAuthorlizeAttribute(Roles ="Admin ,Khách Hàng")]
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
        [CustomAuthorlizeAttribute(Roles = "Admin ,Khách Hàng")]
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
            List<Supplier> listS =db.Suppliers.ToList();
            ViewBag.Supplier1 = new SelectList(listS, "IDSupplier", "NameSupplier");
            List<Brand> listB =db.Brands.ToList();
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
        public ActionResult Detail(string id)
        {
            var D_dongho = db.Watches.FirstOrDefault(m => m.IDWatch == id);
            var Supplier = db.Suppliers.FirstOrDefault(m => m.IDSupplier == D_dongho.IDSupplier);
            var brand = db.Brands.FirstOrDefault(m => m.IDBrand == D_dongho.IDBrand);
            var origin = db.Origins.FirstOrDefault(m => m.IDOrigin == D_dongho.IDOrigin);
            var productFor = db.ProductFors.FirstOrDefault(m => m.IDProductFor == D_dongho.IDProductFor);
            if (D_dongho == null)
            {
                return HttpNotFound();
            }
            var View_watch = new ViewWatch
            {
                Watch = D_dongho,
                SupplierName = Supplier?.NameSupplier,
                Brand=brand?.NameBrand,
                Origin=origin?.NameOrigin,
                ProductFor = productFor?.NameProductFor,
            };
            return View(View_watch);
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