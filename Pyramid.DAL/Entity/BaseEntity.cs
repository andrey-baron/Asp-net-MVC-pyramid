using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.DAL.Entity
{
    public class BaseEntity
    {
        [StringLength(1000)]
        [Display(Name = "Мета-тег description")]
        public string MetaDescription { get; set; }

        [StringLength(1000)]
        [Display(Name = "Тег title")]
        public string MetaTitle { get; set; }
       
        [StringLength(1000)]
        [Display(Name = "Мета-тег keywords")]
        public string MetaKeywords { get; set; }

        [Display(Name = "Готов SEO")]
        public bool IsSEOReady { get; set; }

        [Display(Name = "Алиас")]
        [StringLength(1000)]
        public string Alias { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime DateCreation { get; set; }

        [Display(Name = "Дата изменения")]
        public DateTime DateChange { get; set; }
    }
}
