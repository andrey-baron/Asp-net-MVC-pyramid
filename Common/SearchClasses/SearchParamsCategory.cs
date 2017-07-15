using Common.Models;
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
        public string SearchString { get; set; }

        public IEnumerable< FilterSearchModel> FiltersSearch { get; set; }

        public bool ExistProductsInBd { get; set; }

        public SearchParamsCategory(string searchString, int? id,int? maxPrice=null, int? minPrice=null, IEnumerable<FilterSearchModel> filtersSearch = null,
             int startIndex = 0, int? objectsCount = null) :base(startIndex,objectsCount)
        {
            Id = (int)id;
            MaxPrice = maxPrice;
            MinPrice = minPrice;
             FiltersSearch = filtersSearch;
            SearchString = searchString;
        }
        public SearchParamsCategory(int? id=null,int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
        {
            if (id.HasValue)
            {
                Id = (int)id;
            }
            
        }
    }
}
