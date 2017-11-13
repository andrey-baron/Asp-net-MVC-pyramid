using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class Seo
    {
        public int Id { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        [RegularExpression(@"^[a-z0-9-]+$")]
        [Display(Name = "SEO friendly url: only lowercase, number and dash (-) character allowed")]
        public string Alias { get; set; }
    }
}
