using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public class FeedBack
    {
        [Display(Name = "Имя")]
        [Required(ErrorMessage ="Введите пожалуйста имя")]
        public string Name { get; set; }

        [Display(Name = "Телефон")]
        [Required(ErrorMessage ="Введите пожалуйста телефон")]

        public string Phone { get; set; }

        [Display(Name = "E-mail")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\..+", ErrorMessage ="Ввидите корректный E-mail")]
        public string Email { get; set; }
        
       
    }
}
