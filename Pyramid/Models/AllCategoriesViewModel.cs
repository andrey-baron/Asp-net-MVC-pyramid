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

        public static IEnumerable<AllCategoriesViewModel> ToModelEnumerable(IEnumerable<DBFirstDAL.DataModels.RootCategory> dbRootCategories)
        {
            var model = dbRootCategories.Select(s=>new AllCategoriesViewModel()
            {
                Category=s.Category,
                SubCategories=s.SubCategories
            }).ToList() ;
            return model;
        }
    }
}