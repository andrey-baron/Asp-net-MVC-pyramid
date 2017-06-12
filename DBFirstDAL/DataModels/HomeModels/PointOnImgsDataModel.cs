using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.DataModels.HomeModels
{
    public class PointOnImgsDataModel
    {
        public int Id { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public int BannerId { get; set; }

        public ProductHomeModel Products { get; set; }
    }
}
