using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HW6.Models
{
    public class OrderVM
    {
        public DateTime orderdate { get; set; }
        public string category { get; set; }
        public int Id { get; set; }
        public List<product> Products { get; set; }
        public product Product { get; set; }
        public decimal Total { get; set; }  
        public int quantity { get; set; }
        public decimal price { get; set; }   
        

    }
}