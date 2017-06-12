using Pyramid.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Models.CategoryModels
{
    public class HeaderCategoryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public Image Thumbnail { get; set; }
    }
}