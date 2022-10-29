using HW6.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Collections;
using System.Data.Entity;

namespace HW6.Controllers
{
    public class HomeController : Controller
    {
        private readonly BikeStoresEntities1 db = new BikeStoresEntities1();
        public ActionResult Index()
        {
            return View();           
        }

        
    
    }
}