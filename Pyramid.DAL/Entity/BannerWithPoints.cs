using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.DAL.Entity
{
    public class BannerWithPoints
    {
        public int Id { get; set; }
        [MaxLength(1000)]
        public string PathToImg { get; set; }
        public virtual PointOnImg Points { get; set; }
    }
}
