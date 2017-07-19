using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class GlobalOptionEntity
    {
        public int Id { get; set; }
        public bool IsHtml { get; set; }
        public string StringKey { get; set; }
        [Display(Name ="Название опции")]
        public string Description { get; set; }
        [Display(Name = "Значение опции")]
        public string OptionContent { get; set; }
    }
}
