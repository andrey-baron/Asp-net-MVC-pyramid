using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
   public class Filter
    {
        public int Id { get; set; }
        [Display(Name ="Название")]
        public string Title { get; set; }

       
        public ICollection<Category> Categories { get; set; }
        public ICollection<EnumValue> EnumValues { get; set; }
        public Filter()
        {
            EnumValues = new List<EnumValue>();
        }
    }
}
