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
    public class EventBannerRepository : GenericRepository<EventBanners, PyramidFinalContext, Pyramid.Entity.EventBanner, SearchParamsBase, int>
    {
        public override EventBanner ConvertDbObjectToEntity(PyramidFinalContext context, EventBanners dbObject)
        {
            var banner = new EventBanner() {
                Id=dbObject.Id,
                Thumbnail= Convert.ConvertImageToEntity.Convert(dbObject.Images),
                Title=dbObject.Title
            };
            return banner;
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, EventBanners dbEntity, EventBanner entity, bool exists)
        {
            dbEntity.Title = entity.Title;
        }
        public override void UpdateAfterSaving(PyramidFinalContext dbContext, EventBanners dbEntity, EventBanner entity, bool exists)
        {
            dbEntity.ImageId = entity.Thumbnail.Id;
        }

        protected override IQueryable<EventBanners> BuildDbObjectsList(PyramidFinalContext context, IQueryable<EventBanners> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        protected override EventBanners GetDbObjectByEntity(DbSet<EventBanners> objects, EventBanner entity)
        {
            return objects.FirstOrDefault(f => f.Id == entity.Id);
        }

        protected override Expression<Func<EventBanners, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;
        }
    }
}
