using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class HomeEntity
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Display(Name = "Текст на слайде")]
        public string Content { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Product> Products { get; set; }
        public BannerWithPoints BannerWithPoints { get; set; }
        public FAQ Faq { get; set; }
        [Display(Name = "Название видео-гида")]
        public string TitleVideoGuide { get; set; }
        [Display(Name = "Ссылка на YouTobe")]
        public string LinkYouTobe { get; set; }
        [Display(Name = "Призыв к действию")]
        public string CallToAction { get; set; }
        public Nullable<int> ThumbnailId { get; set; }
        public Image Images { get; set; }
    }
}
