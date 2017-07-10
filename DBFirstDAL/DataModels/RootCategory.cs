using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.DataModels
{
   public class RootCategory
    {
        public Pyramid.Entity.Category Category { get; set; }
        public IEnumerable<Pyramid.Entity.Category> SubCategories { get; set; }

    }
}
