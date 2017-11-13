using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SearchClasses
{
    public class SearchParamsRouteItem : SearchParamsBase
    {
        public SearchParamsRouteItem(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount) {

        }
    }
}
