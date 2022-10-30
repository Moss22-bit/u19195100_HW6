using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using u19195100_HW6.Models;
using u19195100_HW6.Models.ViewModels;
using PagedList;

namespace u19195100_HW6.Controllers
{
    public class OrdersController : Controller
    {

        private BikeStoresEntities db = new BikeStoresEntities();
        // GET: Orders
        //Returning view of placed orders
        public ActionResult Index()
        {
            List<OrderVM> order = db.order_items.Select(p => new OrderVM { orderID = p.order_id, Category = p.product.category.category_name, ProductNames = db.products.Where(x => x.product_id == p.product_id).FirstOrDefault(), Quantity = p.quantity, Price = p.list_price, Total = (p.list_price * p.quantity), date = db.orders.Where(o => o.order_id == p.order_id).FirstOrDefault().order_date }).ToList();
            return View(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //Method for searching the items using date picker
        public ActionResult Search(DateTime date)
        {
            string daysoftheweek = date.ToShortDateString();
            List<OrderVM> myOrders = db.order_items.Where(y => y.order.order_date <= date).Select(p => new OrderVM { orderID = p.order_id, Category = p.product.category.category_name, ProductNames = db.products.Where(x => x.product_id == p.product_id).FirstOrDefault(), Quantity = p.quantity, Price = p.list_price, Total = (p.list_price * p.quantity), date = db.orders.Where(o => o.order_id == p.order_id).FirstOrDefault().order_date }).ToList();
            return View("Index", myOrders);
        }

        
    }

}
