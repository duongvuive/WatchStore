using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchStore.Models
{
    public class ViewWatch
    {
        public Watch Watch { get; set; }
        public string SupplierName { get; set; }
        public string Brand { get; set; }
        public string Origin { get; set; }
        public string ProductFor { get; set; }
    }
}