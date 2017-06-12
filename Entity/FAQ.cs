
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class FAQ
    {
        public int Id { get; set; }
        [Display(Name ="Название")]
        public string Title { get; set; }
        public ICollection<QuestionAnswer> QuestionAnswer { get; set; }

        public FAQ()
        {
            QuestionAnswer = new List<QuestionAnswer>();
        }
    }
}
