using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        [Display(Name = "Название товара")]
        public string Title { get; set; }

        public int? ThumbnailId { get; set; }
        [Display(Name = "Цена")]
        public double Price { get; set; }
        //public bool Available { get; set; }
        [Display(Name = "Тип цены")]
        public Enumerable.TypeProductPrice TypePrice { get; set; }
        [Display(Name = "В наличии")]
        public bool InStock { get; set; }

        public Image ThumbnailImg { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<ProductValue> ProductValues { get; set; }

        public ICollection<EnumValue> EnumValues { get; set; }


        public Product()
        {
            Categories = new List<Category>();
            Images = new List<Image>();
            ProductValues = new List<ProductValue>();
            EnumValues = new List<EnumValue>();
        }
    }
}
