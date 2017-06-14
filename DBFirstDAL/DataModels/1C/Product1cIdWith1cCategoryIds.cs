using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.DataModels._1C
{
    public class Product1cIdWith1cCategoryIds
    {
        public string OneCId { get; set; }

        public IEnumerable<string> CategoryIds { get; set; }
        public Product1cIdWith1cCategoryIds()
        {
            CategoryIds = new List<string>();
        }
    }
}
