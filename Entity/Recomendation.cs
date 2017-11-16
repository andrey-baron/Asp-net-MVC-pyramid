using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
   
    public class Recommendation
    {
        public Recommendation()
        {
            //this.Categories = new List<Category>();
          
        }

        public int Id { get; set; }
        [Display(Name ="Название")]
        public string Title { get; set; }
        [Display(Name = "Описание")]
        public string Content { get; set; }
        [Display(Name = "Краткое описание")]
        public string ShortContent { get; set; }

        // public ICollection<Category> Categories { get; set; }
        [Display(Name = "Картинка")]
        public Image Image { get; set; }

        public int? SeoId { get; set; }

        public Seo Seo { get; set; }

        public string FriendlyUrl { get; set; }
    }
}
