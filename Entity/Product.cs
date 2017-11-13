using Entity;
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
        public string OneCId { get; set; }

        [Display(Name = "Название товара")]
        public string Title { get; set; }

        public int ThumbnailId { get; set; }
        [Display(Name = "Цена")]
        public double Price { get; set; }
        //public bool Available { get; set; }
        [Display(Name = "Тип цены")]
        public Common.TypeProductPrice TypePrice { get; set; }
        [Display(Name = "Описание")]
       
         public string Content { get; set; }

        [Display(Name = "Сезонное предложение")]
        public bool SeasonOffer { get; set; }
        [Display(Name = "Приоритетный")]
        public bool IsPriority { get; set; }
        [Display(Name = "Заполнен")]
        public bool IsFilled { get; set; }
        [Display(Name = "Нет в выгрузке")]
        public bool IsNotUnloading1C { get; set; }

        [Display(Name = "Статус товара")]
        public Common.TypeStatusProduct TypeStatusProduct { get; set; }

        public Nullable<int> SeoId { get; set; }
        public  Seo Seo { get; set; }

        public Image ThumbnailImg { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<ProductValue> ProductValues { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public ICollection<EnumValue> EnumValues { get; set; }
        public string FriendlyUrl { get; set; }
                                           // public ICollection<Recommendation> Recommendations { get; set; }

        public int PopularCount { get; set; }

        public Product()
        {
            Categories = new List<Category>();
            Images = new List<Image>();
            ProductValues = new List<ProductValue>();
            EnumValues = new List<EnumValue>();
            Reviews = new List<Review>();
        }
    }
}
