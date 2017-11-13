using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string OneCId { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Название")]
        public string Title { get; set; }
        //public int? ThumbnailId { get; set; }
        public bool Checked { get; set; }
        [Display(Name ="Родительская категория")]
        public int? ParentId { get; set; }
        public bool FlagRoot { get; set; }

        [Display(Name = "Описание")]
        public string Content { get; set; }
        public Nullable<int> SeoId { get; set; }

        public  Seo Seo { get; set; }
        [Display(Name = "Показывать на сайте")]
        public bool ShowCategoryOnSite { get; set; }

        public string FriendlyUrl { get; set; }

        public Image Thumbnail { get; set; }
        public List<Product> Products { get; set; }
        public ICollection<Filter> Filters { get; set; }
        public ICollection<Recommendation> Recommendations { get; set; }
        public Category()
        {
            Products = new List<Product>();
            Filters = new List<Filter>();
            Thumbnail = new Image();
                Recommendations= new List<Recommendation>(); ;
        }
    }
}
