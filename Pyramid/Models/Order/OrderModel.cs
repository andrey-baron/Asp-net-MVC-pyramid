using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Models.Order
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public Entity.Enumerable.TypeProgressOrder TypeProgressOrder { get; set; }

        public IEnumerable<ProductOrderModel> Products { get; set; }

        public OrderModel()
        {
            Products = new List<ProductOrderModel>();
        }
    }
}