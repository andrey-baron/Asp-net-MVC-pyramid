
using Pyramid.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Models.CategoryModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Image Thumbnail { get; set; }
        public List<Product>  Products { get; set; }
        public List<CategoryFilterViewModel> Filters { get; set; }

        public int MaxPrice { get; set; }
        public int MinPrice { get; set; }

        public int CurrentMaxPrice { get; set; }
        public int CurrentMinPrice { get; set; }

        public CategoryViewModel()
        {
            Products = new List<Product>();
            Filters = new List<CategoryFilterViewModel>();
        }

    }
}