using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public enum TypeStatusProduct
    {
        [Display(Name = "Показывать товар в каталоге")]
        Normal =0,

        [Display(Name = "Скрывать товар в каталоге")]
        Hide = 1,

        [Display(Name = "Показывать как Возможно заказать")]
        WillBeAdded = 2
    }
}
