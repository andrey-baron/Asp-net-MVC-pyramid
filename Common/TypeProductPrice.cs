using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum TypeProductPrice
    {
        [Display(Name = "Простая цена")]
        SimplePrice = 0,
        [Display(Name = "Новинка")]
        NewPrice = 1,

        [Display(Name = "Специальное предложение")]
        SpecialOffer = 3,
    }
}
