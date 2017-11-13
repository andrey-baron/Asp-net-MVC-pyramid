using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SearchClasses
{
    public class SearchParamsImage : SearchParamsBase
    {
        public int? ProductId { get; set; }
        public int? TypeImage { get; set; }

        public SearchParamsImage(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount) {

        }
    }
}
