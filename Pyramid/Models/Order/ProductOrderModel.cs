using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Models.Order
{
    public class ProductOrderModel
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
    }
}