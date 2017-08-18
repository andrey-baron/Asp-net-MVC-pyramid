using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pyramid.Entity;
using Common.SearchClasses;
using System.Data.Entity;
using System.Linq.Expressions;

namespace DBFirstDAL.Repositories
{
    public class BannersOnHomePageRepository : GenericRepository<BannersOnHomePage, PyramidFinalContext, Pyramid.Entity.BannersOnHomePage, SearchParamsBase, int>
    {
        public override Pyramid.Entity.BannersOnHomePage ConvertDbObjectToEntity(PyramidFinalContext context, BannersOnHomePage dbObject)
        {
            var banner = new Pyramid.Entity.BannersOnHomePage()
            {
                Id = dbObject.Id,
                Thumbnail = Convert.ConvertImageToEntity.Convert(dbObject.Images),
                Title = dbObject.Title,
                Link = dbObject.Link,
                Content=dbObject.Content
            };
            return banner;
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, BannersOnHomePage dbEntity, Pyramid.Entity.BannersOnHomePage entity, bool exists)
        {
            dbEntity.Title = entity.Title;
            dbEntity.Link = entity.Link;
            dbEntity.Content = entity.Content;

        }

        public override void UpdateAfterSaving(PyramidFinalContext dbContext, BannersOnHomePage dbEntity, Pyramid.Entity.BannersOnHomePage entity, bool exists)
        {
            dbEntity.ImageId = entity.Thumbnail.Id;
        }

        protected override IQueryable<BannersOnHomePage> BuildDbObjectsList(PyramidFinalContext context, IQueryable<BannersOnHomePage> dbObjects, SearchParamsBase searchParams)
        {
            dbObjects = dbObjects.OrderBy(item => item.Id);
            return dbObjects;
        }

        protected override BannersOnHomePage GetDbObjectByEntity(DbSet<BannersOnHomePage> objects, Pyramid.Entity.BannersOnHomePage entity)
        {
            return objects.FirstOrDefault(f => f.Id == entity.Id);  
        }

        protected override Expression<Func<BannersOnHomePage, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;
        }
    }
}
