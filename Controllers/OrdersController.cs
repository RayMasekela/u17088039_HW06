using HW6.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HW6.Controllers
{
    public class OrdersController : Controller
    {
        private BikeStoresEntities1 db = new BikeStoresEntities1();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Orders(int?i)
        {
            List<order> Orders = new List<order>();
            List<OrderVM> productDatas = db.order_items.Select(p => new OrderVM { Id = p.order_id, category = p.product.category.category_name, Product = db.products.Where(x => x.product_id == p.product_id).FirstOrDefault(), quantity = p.quantity, price = p.list_price, Total = (p.list_price * p.quantity), orderdate = db.orders.Where(o => o.order_id == p.order_id).FirstOrDefault().order_date }).ToList();
           
            
           
            return View(productDatas.ToPagedList(i ?? 1, 10));
        }

        public string GetReports()
        {

            object orders = db.orders.Select(p => new
            {
                Products = db.order_items.Where(y => y.order_id == p.order_id).Select(o => new { category_id = o.product.category.category_id, Quantity = o.quantity, month = p.order_date.Month }).ToList<dynamic>(),
            }).ToList();
            return JsonConvert.SerializeObject(orders);
        }

        public ActionResult Report()
        {
            return View();
        }
    }
}