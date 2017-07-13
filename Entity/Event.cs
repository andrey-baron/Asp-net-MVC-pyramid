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
        public string Title { get; set; }
        [Display(Name = "Описание")]
        public string Content { get; set; }
        [Display(Name = "Краткое описание")]
        public string ShortContent { get; set; }
        [Display(Name = "Начало срока акции")]
        public System.DateTime DateEventStart { get; set; }
        [Display(Name = "Конец скрока акции")]
        public System.DateTime DateEventEnd { get; set; }
        [Display(Name = "Активная")]
        public bool isActive { get; set; }

        public  Image Image { get; set; }
        public IEnumerable<Product>Products { get; set; }

        public Event()
        {
            Products = new List<Product>();
        }

    }
}
