using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Models.Cart
{
    public class ProductCartModel
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Title { get; set; }
        public Pyramid.Entity.Image Picture { get; set; }
    }
}