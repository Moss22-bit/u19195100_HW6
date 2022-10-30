using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19195100_HW6.Models.ViewModels
{
    public class OrderVM
    {
        public int orderID { get; set; }
        public product ProductNames { get; set; }
        public DateTime date { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}