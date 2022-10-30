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
    public class productsController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();

        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var products = db.products.Include(p => p.brand).Include(p => p.category);
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.product_name.Contains(searchString));
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(products.ToList().ToPagedList(pageNumber, pageSize));
        }

        public JsonResult Edit(int? id)
        {
            product myproduct = db.products.Find(id);
            var Productobject = new product
            {
                product_id = myproduct.product_id,
                product_name = myproduct.product_name,
                category_id = myproduct.category_id,
                brand_id = myproduct.brand_id,
                list_price = myproduct.list_price,
                model_year = myproduct.model_year,
                brands = db.brands.ToList().Select(x => new brand { brand_id = x.brand_id, brand_name = x.brand_name }).ToList(),
                categories = db.categories.ToList().Select(x => new category { category_id = x.category_id, category_name = x.category_name }).ToList()
            };
            return new JsonResult { Data = new { product = Productobject }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult Edit([Bind(Include = "product_id,product_name,brand_id,category_id,model_year,list_price")] product product)
        {
            var message = "";
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    message = "200 OK";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return new JsonResult { Data = new { status = message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Details(int? id)
        {
            product storedProduct = db.products.Where(x => x.product_id == id).FirstOrDefault();
            customerDetails NewProduct = new customerDetails();
            NewProduct.product_name = storedProduct.product_name;
            NewProduct.model_year = storedProduct.model_year;
            NewProduct.list_price = storedProduct.list_price;
            NewProduct.brand_name = storedProduct.brand.brand_name;
            NewProduct.category_name = storedProduct.category.category_name;
            NewProduct.storeQuantities = (
                        from stock in db.stocks.ToList()
                        join store in db.stores.ToList() on stock.store_id equals store.store_id
                        where stock.product_id == id
                        group stock by stock.store.store_name into groupedStores
                        select new customerStore
                        {
                            store_name = groupedStores.Key,
                            quantity = (int)groupedStores.Sum(oi => oi.quantity)
                        }).ToList();
            return new JsonResult { Data = new { product = NewProduct }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult Delete(int? id)
        {
            product StoredProductsInDB = db.products.Where(x => x.product_id == id).FirstOrDefault();
            customerDetails ProductFromDb = new customerDetails();
            ProductFromDb.product_name = StoredProductsInDB.product_name;
            ProductFromDb.model_year = StoredProductsInDB.model_year;
            ProductFromDb.list_price = StoredProductsInDB.list_price;
            ProductFromDb.brand_name = StoredProductsInDB.brand.brand_name;
            ProductFromDb.category_name = StoredProductsInDB.category.category_name;
            return new JsonResult { Data = new { product = ProductFromDb }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult DeleteConfirmed(int id)
        {
            var message = "";
            try
            {
                product product = db.products.Find(id);
                db.products.Remove(product);
                db.SaveChanges();
                message = "";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return new JsonResult { Data = new { product = message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult Create()
        {
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name");
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name");
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,product_name,brand_id,category_id,model_year,list_price")] product product)
        {
            if (ModelState.IsValid)
            {
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

