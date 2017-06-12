using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.DAL.Entity
{
   public class Filter
    {
        public int Id { get; set; }
        public string Title { get; set; }

       
        public virtual Category Category { get; set; }
        public virtual ICollection<FilterValue> FilterValues { get; set; }
        public Filter()
        {
            FilterValues = new List<FilterValue>();
        }
    }
}
