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

        public SearchParamsProduct(string searchString, int? categoryId, bool? priority,
        int startIndex = 0, int? objectsCount = null):base(startIndex, objectsCount)
        {
            CategoryId = categoryId;
            Priority = priority;
            SearchString = searchString;
        }

        SearchParamsProduct(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
        {

        }
    }
}
