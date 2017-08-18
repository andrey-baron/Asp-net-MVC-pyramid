using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class BannersOnHomePage
    {
        public int Id { get; set; }
        [Display(Name = "Текст")]
        public string Content { get; set; }
        [Display(Name = "Название")]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Ссылка")]
        public string Link { get; set; }
        [Display(Name = "Картинка")]
        public Image Thumbnail { get; set; }
    }
}
