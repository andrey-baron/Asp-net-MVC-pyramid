using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.DAL.Entity
{
    public class FilterValue
    {
        public int Id { get; set; }
        public string Value { get; set; }


        public virtual Product Product { get; set; }
        public virtual Filter Filter { get; set; }

    }
}
