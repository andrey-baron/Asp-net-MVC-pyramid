using Common.SearchClasses;
using DBFirstDAL.Intercaces;
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
    public class PageRepository : GenericRepository<Pages, PyramidFinalContext, Pyramid.Entity.Page, SearchParamsBase, int>
    {
        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Pages dbEntity, Page entity, bool exists)
        {
            dbEntity.Content = entity.Content;
            dbEntity.Title = entity.Title;

        }

        protected override IQueryable<Pages> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Pages> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        public override Page ConvertDbObjectToEntity(PyramidFinalContext context, Pages dbObject)
        {
            var page = new Page() {
                Content=dbObject.Content,
                Title=dbObject.Title,
                Id=dbObject.Id
            };
            return page;
        }

        protected override Pages GetDbObjectByEntity(DbSet<Pages> objects, Page entity)
        {
            return objects.FirstOrDefault(f => f.Id == entity.Id);
        }

        protected override Expression<Func<Pages, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;
        }
    }
}
