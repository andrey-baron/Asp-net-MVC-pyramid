using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.DataModels
{
    public class CategoryWithThumbnail
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public Images Thumbnail { get; set; }
    }
}
