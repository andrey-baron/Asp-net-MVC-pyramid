using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SearchClasses
{
    public class SearchParamsBase
    {
        public int StartIndex { get; set; }
        public int? ObjectsCount { get; set; }

        public SearchParamsBase(int startIndex = 0, int? objectsCount = null)
        {
            StartIndex = startIndex;
            ObjectsCount = objectsCount;
        }
    }
}
