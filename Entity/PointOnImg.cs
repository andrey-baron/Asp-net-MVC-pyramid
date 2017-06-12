using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class PointOnImg
    {
        public int Id { get; set; }
        public double CoordX { get; set; }
        public double CoordY { get; set; }
        public Product  Products { get; set; }
    }
}
