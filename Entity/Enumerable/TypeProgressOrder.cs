using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity.Enumerable
{
   public enum TypeProgressOrder
    {
        [Display(Name = "Новый заказ")]
        SimplePrice = 0,
        [Display(Name = "Подтвержденный заказ")]
        BestPrice = 1,
    }
}
