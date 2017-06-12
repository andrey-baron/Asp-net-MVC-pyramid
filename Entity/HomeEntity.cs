using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class HomeEntity
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Title { get; set; }
        public Category Category { get; set; }
        public BannerWithPoints BannerWithPoints { get; set; }
        public FAQ Faq { get; set; }
        public ICollection<VideoGuide> VideoGuide { get; set; }
    }
}
