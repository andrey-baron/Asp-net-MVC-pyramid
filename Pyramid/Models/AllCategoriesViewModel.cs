using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Models
{
    public class AllCategoriesViewModel
    {
        public Entity.Category Category { get; set; }
        public IEnumerable<Entity.Category> SubCategories { get; set; }
    }
}