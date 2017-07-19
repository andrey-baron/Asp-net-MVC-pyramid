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
    public class ReviewRepository : GenericRepository<Reviews  , PyramidFinalContext, Pyramid.Entity.Review, SearchParamsReview, int>
    {
        
        public void ToNotApproved(int reviewId)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                var efReview = dbContext.Reviews.Find(reviewId);
                if (efReview != null)
                {
                    efReview.IsApproved = false;
                    dbContext.SaveChanges();
                }
            }
           
        }
        public void UpdateApproved(int reviewId, bool currentApproved)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efReview = dbContext.Reviews.Find(reviewId);
                if (efReview != null)
                {
                    efReview.IsApproved = currentApproved;
                    dbContext.SaveChanges();
                }
            }
        }

        public IEnumerable<Review> GetReviewsByProductId(int productId,SearchParamsBase searchParams)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var objects=dbContext.Reviews.Where(w => w.ProductId == productId).Select(s=> ConvertDbObjectToEntity(dbContext,s));
                return objects;
            }

        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Reviews dbEntity, Review entity, bool exists)
        {
            dbEntity.Content = entity.Content;
            dbEntity.DateCreation = entity.DateCreation;
            dbEntity.IsApproved = entity.IsApproved;
            dbEntity.IsRead = entity.IsRead;
            dbEntity.Name = entity.Name;
            dbEntity.ProductId = entity.ProductId;
            dbEntity.Rating = entity.Rating;
        }

        protected override Reviews GetDbObjectByEntity(DbSet<Reviews> objects, Review entity)
        {
            return objects.Find(entity.Id);
        }

        protected override Expression<Func<Reviews, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;
        }

        public override Review ConvertDbObjectToEntity(PyramidFinalContext context, Reviews dbObject)
        {
            var review = new Review()
            {
                Content = dbObject.Content,
                DateCreation = dbObject.DateCreation,
                Id = dbObject.Id,
                IsApproved = dbObject.IsApproved,
                IsRead = dbObject.IsRead,
                Name = dbObject.Name,
                Product = new Product()
                {
                    Title = dbObject.Products.Title,
                    Id = dbObject.Products.Id,

                },
                ProductId = dbObject.ProductId,
                Rating = dbObject.Rating,
            };
            return review;
        }

        protected override IQueryable<Reviews> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Reviews> dbObjects, SearchParamsReview searchParams)
        {
            if (searchParams.ProductTitle!=null)
            {
                dbObjects = dbObjects.Where(w => w.Products.Title.Contains(searchParams.ProductTitle));
            }
            if (searchParams.IsApproved.HasValue)
            {
                dbObjects = dbObjects.Where(w => w.IsApproved == searchParams.IsApproved.Value);
            }
            if (searchParams.isNotRead.HasValue)
            {
                dbObjects = dbObjects.Where(w => !w.IsRead);
            }
            return dbObjects;
        }
    }
}
