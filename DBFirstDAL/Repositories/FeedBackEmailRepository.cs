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
    public class FeedBackEmailRepository : GenericRepository<FeedBackEmails, PyramidFinalContext, Pyramid.Entity.FeedBack, SearchParamsBase, int>
    {
        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, FeedBackEmails dbEntity, FeedBack entity, bool exists)
        {
            dbEntity.Email = entity.Email;
        }

        protected override IQueryable<FeedBackEmails> BuildDbObjectsList(PyramidFinalContext context, IQueryable<FeedBackEmails> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        public override FeedBack ConvertDbObjectToEntity(PyramidFinalContext context, FeedBackEmails dbObject)
        {
            var feedback = new FeedBack() {
            Email=dbObject.Email,
            Id=dbObject.Id
            };
            return feedback;

        }

        protected override FeedBackEmails GetDbObjectByEntity(DbSet<FeedBackEmails> objects, FeedBack entity)
        {
            return objects.FirstOrDefault(f => f.Id == entity.Id);
        }

        protected override Expression<Func<FeedBackEmails, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;
        }
    }
}
