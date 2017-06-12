using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.DataModels.HomeModels
{
    public class ProductHomeModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int TypePrice { get; set; }
        public bool InStock { get; set; }
        public bool SeasonOffer { get; set; }
        public Images ThumbnailImg { get; set; }
    }
}
