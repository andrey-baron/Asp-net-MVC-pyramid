using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum TypeFilledProduct
    {
        
        [Display(Name = "Заполнен")]
        IsFilled =1,
        [Display(Name = "Не заполнен")]
        NotFilled =2
    }
}
