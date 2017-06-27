using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class Order
    {
        public int Id { get; set; }
        [Display(Name = "Имя")]
        public string UserName { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Адрес доставки")]
        public string Adress { get; set; }
        [Display(Name = "Состояние заказа")]
        public Enumerable.TypeProgressOrder TypeProgressOrder { get; set; }

        public  IEnumerable<Product> Products { get; set; }

        public Order()
        {
            Products = new List<Product>();
        }
    }
}
