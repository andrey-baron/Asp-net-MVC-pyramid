using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class BaseEntity
    {
       

       
       
      

        [Display(Name = "Готов SEO")]
        public bool IsSEOReady { get; set; }

     

        [Display(Name = "Дата создания")]
        public DateTime DateCreation { get; set; }

        [Display(Name = "Дата изменения")]
        public DateTime DateChange { get; set; }
    }
}
