using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class BannerWithPoints
    {
        public int BannerId { get; set; }
        public Image Images { get; set; }
        public List< PointOnImg> PointOnImgs { get; set; }

        public BannerWithPoints()
        {
            PointOnImgs = new List<PointOnImg>();
        }
    }
}
