using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pyramid.Models.Order
{
    public class OrderModel
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

        public Entity.Enumerable.TypeProgressOrder TypeProgressOrder { get; set; }

        public IEnumerable<ProductOrderModel> Products { get; set; }

        public OrderModel()
        {
            Products = new List<ProductOrderModel>();
        }
    }
}