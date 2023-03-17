using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;

namespace WatchStore.Controllers
{
    public class KhachHangController : Controller
    {
        dbDongHoDataContext data = new dbDongHoDataContext("Data Source=DESKTOP-NEIOBVT;Initial Catalog=WatchStore;Integrated Security=True");
        public ActionResult DangKyThongTin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKyThongTin(FormCollection collection, Customer kh)
        {
            string email = Session["Email"].ToString();
            var GetId = data.AspNetUsers.First(m=>m.Email == email);
            var IDCustomer = from b in data.AUTO_IDCustomer() select b;
            var FullName = collection["FullName"];
            var Gender = collection["Gender"];
            var CitizenIdentification = collection["CitizenIdentification"];
            var Address = collection["Address"];
            

            foreach (var item in IDCustomer)
            {
                kh.IDCustomer += item;
            }

            kh.FullName = FullName;
            kh.Gender = Gender;
            kh.CitizenIdentification = CitizenIdentification;
            kh.Address = Address;
            kh.Id = GetId.Id.ToString();
            kh.IDAP = 1;
            data.Customers.InsertOnSubmit(kh);
            data.SubmitChanges();
            return RedirectToAction("Index","Watch");

            /*   return this.DangKyThongTin();*/

        }
    }
}