﻿
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
        public List<Product>  Products { get; set; }
        public List<CategoryFilterViewModel> Filters { get; set; }

        public CategoryViewModel()
        {
            Products = new List<Product>();
            Filters = new List<CategoryFilterViewModel>();
        }

    }
}