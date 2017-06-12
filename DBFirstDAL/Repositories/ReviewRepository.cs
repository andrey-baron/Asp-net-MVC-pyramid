using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.Repositories
{
    public class ReviewRepository : GenericRepository<PyramidFinalContext, Review>
    {
        public IQueryable<Review> GetNewReviews()
        {
            return FindBy(i => i.IsRead == false);
        }

        public IQueryable<Review> GetApprovedReviews()
        {
            return FindBy(i => i.IsApproved == true);
        }
        public IQueryable<Review> GetNotApprovedReviews()
        {
            return FindBy(i => i.IsApproved == false);
        }
        public void ToNotApproved(int reviewId)
        {
            var efReview = FindBy(i => i.Id == reviewId).SingleOrDefault();
            if (efReview!=null)
            {
                efReview.IsApproved = false;
                Save();
            }
        }
        public void UpdateApproved(int reviewId, bool currentApproved)
        {
            var efReview = FindBy(i => i.Id == reviewId).SingleOrDefault();
            if (efReview != null)
            {
                efReview.IsApproved = currentApproved;
                Save();
            }
        }

        public IQueryable<Review> GetReviewsByProductId(int productId)
        {
            return FindBy(i => i.ProductId == productId&& i.IsApproved==true);
        }
    }
}
