
using Entity;
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

        public List<CategoryShortViewModel> NestedCategories { get; set; }

        public int MaxPrice { get; set; }
        public int MinPrice { get; set; }

        public int CurrentMaxPrice { get; set; }
        public int CurrentMinPrice { get; set; }
        public string Content { get; set; }
        public Seo Seo { get; set; }

        public bool ExistProducts { get; set; }

        public CategoryViewModel()
        {
            Products = new List<Product>();
            Filters = new List<CategoryFilterViewModel>();
            NestedCategories = new List<CategoryShortViewModel>();
        }

        public static CategoryViewModel ToModel(Category category)
        {
            var model = new CategoryViewModel() {
                MaxPrice = category.Products.Count>0?(int)category.Products.Max(m=>m.Price):0,
                MinPrice = category.Products.Count > 0 ? (int)category.Products.Min(m => m.Price):0,
                Products=category.Products,
                Filters=category.Filters.Select(s=>new CategoryFilterViewModel()
                {
                    Id =s.Id,
                    Title=s.Title,
                    EnumValues=s.EnumValues.Select(e=>new CategoryEnumValueViewModel()
                    {
                        Checked=false,
                        Id=e.Id,
                        Key=e.Key
                    }).ToList()
                }).ToList(),
                Id=category.Id,
                Thumbnail=category.Thumbnail,
                Title=category.Title,
                Seo=category.Seo,
                Content= category.Content

            };
            return model;
        }

    }
}