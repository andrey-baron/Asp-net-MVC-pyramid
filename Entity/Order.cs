using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public int TypeProgressOrder { get; set; }

        public  IEnumerable<Product> Products { get; set; }

        public Order()
        {
            Products = new List<Product>();
        }
    }
}
