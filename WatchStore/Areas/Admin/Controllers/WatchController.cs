using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;

namespace WatchStore.Areas.Admin.Controllers
{
    public class WatchController : Controller
    {
        // GET: Admin/Watch
        

        public ActionResult Index()
        {
            return View();
        }
        
    }
}