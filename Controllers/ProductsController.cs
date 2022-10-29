using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HW6.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PagedList;

namespace HW6.Controllers
{
    public class ProductsController : Controller
    {
        private BikeStoresEntities1 db = new BikeStoresEntities1();

        // GET: Products
        public ActionResult Index(string search, int? i)
        {
            var products = db.products.Include(p => p.brand).Include(p => p.category);
            if (search != null)
            {
                products = db.products.Where(x => x.product_name.Contains(search)).Include(p => p.brand).Include(p => p.category);
            }
            return View(products.ToList().ToPagedList(i??1, 10));
        }

        // GET: Products/Details/5
        public ActionResult Details()
        {
           return PartialView();
        }

        // GET: Products/Create
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

        // GET: Products/Edit/5
        public ActionResult EditView()
        {
            return PartialView();
        }
        public string GetProductId(int? id)
        {
            
            db.Configuration.ProxyCreationEnabled = false;
           //product product = db.products.Find(id);
            object product = db.products.Where(x => x.product_id == id).Select(p => new { id = p.product_id, name = p.product_name, brand = p.brand.brand_name, catergory = p.category.category_name, catergoryId = p.category.category_id, model = p.model_year, price = p.list_price, brandId = p.brand.brand_id }).FirstOrDefault();
           
          return JsonConvert.SerializeObject(product); 
        }

        public string Brands()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<brand> brands = db.brands.ToList();
            return JsonConvert.SerializeObject(brands);
        }


        public string Categories()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<category> categories = db.categories.ToList();
            return JsonConvert.SerializeObject(categories);
        }

        [HttpPost]        
        public ActionResult Edit(int id, string name, int brand, int category, short year, decimal price)
        {
            product product = new product
            {
                product_id = id,
                product_name = name,
                brand_id = brand,
                category_id = category, 
                model_year = year,
                list_price = price

            };

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return RedirectToAction("Index");
        }

        public string ProductDetails(int id)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            object productDetial = db.stocks.Where(y => y.product_id == id).Include(v => v.product).Select(p => new {
                productname = p.product.product_name,
                year = p.product.model_year,
                price = p.product.list_price,
                brand = p.product.brand.brand_name,
                catergory = p.product.category.category_name,
                stores = db.stocks.Where(s => s.product_id == id).Select(n => new { storename = n.store.store_name, quantity = n.quantity })
            }).FirstOrDefault();

            return JsonConvert.SerializeObject(productDetial);

        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return PartialView();
        }
     
        public string DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return JsonConvert.SerializeObject(product);
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
