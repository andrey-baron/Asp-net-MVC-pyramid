using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class Event
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Описание")]
        public string Content { get; set; }
        [Display(Name = "Краткое описание")]
        public string ShortContent { get; set; }
        [Display(Name = "Начало срока акции")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime DateEventStart { get; set; }
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Конец скрока акции")]
        public System.DateTime DateEventEnd { get; set; }
        [Display(Name = "Активная")]
        [Required]
        public bool isActive { get; set; }

        public  Image Image { get; set; }
        public IEnumerable<Product>Products { get; set; }

        public int? SeoId { get; set; }

        public Seo Seo { get; set; }

        public string FriendlyUrl { get; set; }

        public Event()
        {
            Products = new List<Product>();
        }

    }
}
