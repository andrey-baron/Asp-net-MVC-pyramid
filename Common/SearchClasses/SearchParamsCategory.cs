using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SearchClasses
{
    public class SearchParamsCategory: SearchParamsBase 
    {
        public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }
        public IEnumerable<int> EnumValueIds { get; set; }
        public int Id { get; set; }

        public bool ExistProductsInBd { get; set; }

        public SearchParamsCategory(int id,int? maxPrice=null, int? minPrice=null, IEnumerable<int> enumValueIds=null,
             int startIndex = 0, int? objectsCount = null) :base(startIndex,objectsCount)
        {
            Id = id;
            MaxPrice = maxPrice;
            MinPrice = minPrice;
            EnumValueIds = enumValueIds;
        }
    }
}
