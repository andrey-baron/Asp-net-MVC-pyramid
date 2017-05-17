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
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int? ThumbnailId { get; set; }
        public bool Cheaked { get; set; }
        [Display(Name ="Родительская категория")]
        public int? ParentId { get; set; }

        public Image Thumbnail { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Filter> Filters { get; set; }
        public Category()
        {
            Products = new List<Product>();
            Filters = new List<Filter>();
        }
    }
}
