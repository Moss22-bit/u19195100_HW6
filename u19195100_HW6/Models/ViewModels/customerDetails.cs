using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19195100_HW6.Models.ViewModels
{
    public class customerDetails
    {
        public string product_name { get; set; }
        public short model_year { get; set; }
        public decimal list_price { get; set; }
        public string brand_name { get; set; }
        public string category_name { get; set; }
        public List<customerStore> storeQuantities { get; set; }
    }
}