using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SearchClasses
{
    public class SearchParamsProduct: SearchParamsBase
    {
        public string SearchString { get; set; }
        public int? CategoryId { get; set; }
        public bool? Priority { get; set; }
        public bool? IsSearchOnlyPublicProduct { get; set; }
        public int? Filled { get; set; }

        public SearchParamsProduct(string searchString, int? categoryId, bool? priority, int filled,
        int startIndex = 0, int? objectsCount = null):base(startIndex, objectsCount)
        {
            CategoryId = categoryId;
            Priority = priority;
            SearchString = searchString;
            IsSearchOnlyPublicProduct = false;
            Filled = filled;
        }
        public SearchParamsProduct(string searchString, 
        int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
        {
            SearchString = searchString;
            IsSearchOnlyPublicProduct = false;

        }

        public SearchParamsProduct(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
        {
            IsSearchOnlyPublicProduct = false;

        }
    }
}
