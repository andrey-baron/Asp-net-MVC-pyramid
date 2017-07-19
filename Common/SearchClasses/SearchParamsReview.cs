using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SearchClasses
{
    public class SearchParamsReview :SearchParamsBase
    {
        public bool? IsApproved { get; set; }
        public bool? isNotRead { get; set; }
        public string ProductTitle { get; set; }

        public int? ProductId { get; set; }

        public SearchParamsReview(string productTitle,bool? isApproved,bool? isRead,
            int startIndex = 0, int? objectsCount = null) :base(startIndex,objectsCount)
        {
            IsApproved = isApproved;
            isNotRead = isRead;
            ProductTitle = productTitle;
        }
        public SearchParamsReview(int? productId,
            int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
        {
            ProductId = productId;
        }
    }

}
