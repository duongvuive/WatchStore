using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WatchStore.Models
{
    [Serializable]
    public class GioHang
    {
        dbDongHoDataContext db = new dbDongHoDataContext("Data Source=FREEDY\\SQLEXPRESS;Initial Catalog=hi;Integrated Security=True");
        private int id;

        public int IDWatch { get; set; }
        [Display(Name = "Mã Đồng hồ")]
        public string NameWatch { get; set; }
        [Display(Name = "Tên Đông Hồ")]
        public string Image { get; set; }
        [Display(Name = "Hình")]
        
        public int iSoLuong { get; set; }
        [Display(Name = "So Luong")]
        public Double Price { get; set; }
        [Display(Name = "Đơn Giá")]
        public Double ThanhTien
        {
            get { return Price; }
        }
        
        
        public GioHang(string IDWatch )
        {
            
            Watch watch = db.Watches.Single(n => n.IDWatch == IDWatch);
            NameWatch = watch.NameWatch;
            Image = watch.Image;
            Price = double.Parse(watch.Price.ToString());
        }

        public GioHang(int id)
        {
            this.id = id;
        }
    }
}