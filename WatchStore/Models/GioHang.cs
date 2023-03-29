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
        dbDongHoDataContext db = new dbDongHoDataContext("Data Source=FREEDY\\SQLEXPRESS;Initial Catalog=DongHo;Integrated Security=True");
        [Display(Name = "Mã Đồng hồ")]
        public string IDWatch { get; set; }
        [Display(Name = "Tên Đông Hồ")]
        public string NameWatch { get; set; }
        [Display(Name = "Hình")]
        public string Image { get; set; }
        
        [Display(Name = "Số Lượng")]
        public int iSoLuong { get; set; }
        [Display(Name = "Đơn Giá")]
        public Double Price { get; set; }
        [Display(Name = "Thành Tiền")]
        public Double ThanhTien
        {
            get { return Price; }
        }
        
        
        public GioHang(string id )
        {
            
            Watch watch = db.Watches.Single(n => n.IDWatch == id);
            NameWatch = watch.NameWatch;
            Image = watch.Image;
            Price = double.Parse(watch.Price.ToString());
            iSoLuong = 1;
        }

       
    }
}