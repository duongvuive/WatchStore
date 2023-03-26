using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;

namespace WatchStore.Controllers
{
    public class FindController : Controller
    {
        // GET: Find
        dbDongHoDataContext data=new dbDongHoDataContext("Data Source=DESKTOP-NEIOBVT;Initial Catalog=WatchStore;Integrated Security=True");
        public ActionResult Search(string ProductName,int?page,int? pageSize,string OrderBy)
        {
           
            if (!string.IsNullOrEmpty(ProductName))
            {
                 if (page == null)
                {
                    page = 1;
                }
                if (pageSize == null)
                {
                    pageSize = 6;
                }
                var s = data.Watches.Where(m => m.NameWatch.Contains(ProductName));
                switch (OrderBy)
                {
                    case "Price_asc":
                        s = s.OrderBy(m => m.Price); break;//tăng dần
                    case "Price_desc":
                        s = s.OrderByDescending(m => m.Price); break;//giảm dần
                    case "Name_asc":
                        s = s.OrderBy(m => m.NameWatch); break;
                    case "Name_desc":
                        s = s.OrderByDescending(m => m.NameWatch); break;
                    default:
                        s = s.OrderBy(m => m.IDWatch);
                        break;
                }
               
                return View(s.ToPagedList((int)page, (int)pageSize));
            }
            else { return RedirectToAction("Index","Watch"); }
        }
    }
}