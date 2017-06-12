using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class Review
    {
        public int Id { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Отзыв")]
        public string Content { get; set; }
        public int ProductId { get; set; }
        [Display(Name = "Рейтинг")]
        public int Rating { get; set; }
        [Display(Name = "Дата отзыва")]
        public System.DateTime DateCreation { get; set; }
        public bool IsRead { get; set; }
        [Display(Name ="Одобрено")]
        public bool IsApproved { get; set; }

        public Product Product { get; set; }
    }
}
