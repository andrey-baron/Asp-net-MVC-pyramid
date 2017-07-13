using Pyramid.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid. Entity
{
    public class EventBanner
    {
        public int Id { get; set; }
        [Display(Name ="Название")]
        public string Title { get; set; }

        public Image Thumbnail { get; set; }
    }
}
