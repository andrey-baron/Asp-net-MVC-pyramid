using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.DataModels.HomeModels
{
    public class BannerWithPointsHomeDataModel
    {
        public int BannerId { get; set; }
        public int ImageId { get; set; }

        public  Images Images { get; set; }
        public  ICollection<PointOnImgsDataModel> PointOnImgs { get; set; }
        public BannerWithPointsHomeDataModel()
        {
            PointOnImgs = new List<PointOnImgsDataModel>();
        }
    }
}
