using Common.SearchClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pyramid.Entity;
using System.Data.Entity;
using System.Linq.Expressions;

namespace DBFirstDAL.Repositories
{
    public class ReviewRepository : GenericRepository<Review, PyramidFinalContext, Pyramid.Entity.Review, SearchParamsBase,int>
    {
        public IEnumerable<Review> GetNewReviews()
        {
            return FindBy(i => i.IsRead == false);
        }

        public IEnumerable<Review> GetApprovedReviews()
        {
            return FindBy(i => i.IsApproved == true);
        }
        public IEnumerable<Review> GetNotApprovedReviews()
        {
            return FindBy(i => i.IsApproved == false);
        }
        public void ToNotApproved(int reviewId)
        {
            var efReview = FindBy(i => i.Id == reviewId).SingleOrDefault();
            if (efReview!=null)
            {
                efReview.IsApproved = false;
               // Save();
            }
        }
        public void UpdateApproved(int reviewId, bool currentApproved)
        {
            var efReview = FindBy(i => i.Id == reviewId).SingleOrDefault();
            if (efReview != null)
            {
                efReview.IsApproved = currentApproved;
               // Save();
            }
        }

        public IEnumerable<Review> GetReviewsByProductId(int productId)
        {
            return FindBy(i => i.ProductId == productId&& i.IsApproved==true);
        }

        protected override Review GetDbObjectByEntity(DbSet<Review> objects, Pyramid.Entity.Review entity)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Review, int>> GetIdByDbObjectExpression()
        {
            throw new NotImplementedException();
        }

        protected override Pyramid.Entity.Review ConvertDbObjectToEntity(PyramidFinalContext context, Review dbObject)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<Review> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Review> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Review dbEntity, Pyramid.Entity.Review entity, bool exists)
        {
            throw new NotImplementedException();
        }
    }
}
