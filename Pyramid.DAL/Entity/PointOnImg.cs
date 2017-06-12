using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.DAL.Entity
{
    public class PointOnImg
    {
        public int Id { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public virtual List<Product> ReferenceProduct { get; set; }
    }
}
