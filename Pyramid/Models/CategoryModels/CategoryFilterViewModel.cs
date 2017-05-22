using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Models.CategoryModels
{
    public class CategoryFilterViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<CategoryEnumValueViewModel> EnumValues { get; set; }
        public CategoryFilterViewModel()
        {
            EnumValues = new List<CategoryEnumValueViewModel>();
        }
    }
}