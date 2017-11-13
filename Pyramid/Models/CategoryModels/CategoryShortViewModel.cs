using Pyramid.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pyramid.Models.CategoryModels
{
    public class CategoryShortViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Title { get; set; }

        public Image Thumbnail { get; set; }
        public string FriendlyUrl { get; set; }

        public static CategoryShortViewModel ToModel(Category category)
        {
            return new CategoryShortViewModel() {
                Id=category.Id,
                Thumbnail=category.Thumbnail,
                Title= category.Title,
                FriendlyUrl=category.FriendlyUrl
            };
        }
    }
}