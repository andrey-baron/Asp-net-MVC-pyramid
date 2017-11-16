using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Common.SearchClasses;
using Pyramid.Entity;
using System.Linq.Expressions;

namespace DBFirstDAL.Repositories
{
   public class EventRepository: GenericRepository<Events,PyramidFinalContext,Pyramid.Entity.Event, SearchParamsBase,int>
    {
        public EventRepository(PyramidFinalContext context):base(context) { }

        public EventRepository() { }
      

        public bool DeleteReletedProduct(int id,int productId)
        {
            using (PyramidFinalContext dbContext=new PyramidFinalContext())
            {
                var actionResult = false;
                var efEvent = dbContext.Events.Find(id);
                if (efEvent != null)
                {
                    if (efEvent.Products != null)
                    {
                        var efProd = efEvent.Products.FirstOrDefault(f => f.Id == productId);
                        if (efProd != null)
                        {
                            efEvent.Products.Remove(efProd);
                            dbContext.SaveChanges();
                            actionResult = true;

                        }
                    }
                }
                return actionResult;
            }
           
        }

        protected override Events GetDbObjectByEntity(DbSet<Events> objects, Event entity)
        {
            return objects.FirstOrDefault(f => f.Id == entity.Id);
        }

        protected override Expression<Func<Events, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;
        }

        public override Event ConvertDbObjectToEntity(PyramidFinalContext context, Events dbObject)
        {
            var seo = new SeoRepository(context).Get(dbObject.SeoId.HasValue ? dbObject.SeoId.Value : 0);
            var products = new ProductRepository(context).Get(new SearchParamsProduct() { EventId=dbObject.Id});
            var entity = new Event() {
                Content= dbObject.Content,
                Id=dbObject.Id,
                DateEventEnd=dbObject.DateEventEnd,
                DateEventStart=dbObject.DateEventStart,
                //Image=dbObject.EventImages.
                isActive=dbObject.isActive,
                Products=products.Objects,
                ShortContent=dbObject.ShortContent,
                Title= dbObject.Title,
                Seo=seo,
                SeoId=dbObject.SeoId,
                FriendlyUrl= new RouteItemRepository(context).GetFriendlyUrl(dbObject.Id,Common.TypeEntityFromRouteEnum.Event)
            };
            if (dbObject.EventImages!=null && dbObject.EventImages.ImageId.HasValue && dbObject.EventImages.Images != null)
            {
                var img = new Image()
                {
                    Id = dbObject.EventImages.Images.Id,
                    ImgAlt = dbObject.EventImages.Images.ImgAlt,
                    ServerPathImg = dbObject.EventImages.Images.ServerPathImg,
                    Title = dbObject.EventImages.Images.Title
                };
                entity.Image = img;
            }
            return entity;
        }

        protected override IQueryable<Events> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Events> dbObjects, SearchParamsBase searchParams)
        {
            return dbObjects;
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Events dbEntity, Event entity, bool exists)
        {
            dbEntity.Content = entity.Content;
            dbEntity.DateEventEnd = entity.DateEventEnd;
            dbEntity.DateEventStart = entity.DateEventStart;
            dbEntity.isActive = entity.isActive;
            dbEntity.ShortContent = entity.ShortContent;
            dbEntity.Title = entity.Title;
        }
        public override void UpdateAfterSaving(PyramidFinalContext dbContext, Events dbEntity, Event entity, bool exists)
        {
            var listProductIds = new List<int>(entity.Products.Select(i => i.Id));

            dbEntity.Products.Clear();
            foreach (var item in listProductIds)
            {
                var efProd = dbContext.Products.Find(item);
                if (efProd!=null)
                {
                    dbEntity.Products.Add(efProd);

                }
            }

            if (dbEntity.Id!=0&& entity.Image.Id!=0)
            {
                dbEntity.EventImages = new EventImages()
                {
                    ImageId = entity.Image.Id,
                    EventId = dbEntity.Id
                };
            }

            if (entity.Seo != null)
            {
                if (dbEntity.Seo == null)
                {
                    dbEntity.Seo = new Seo();
                }
                dbEntity.Seo.Alias = entity.Seo.Alias;
                dbEntity.Seo.MetaTitle = entity.Seo.MetaTitle;
                dbEntity.Seo.MetaKeywords = entity.Seo.MetaKeywords;
                dbEntity.Seo.MetaDescription = entity.Seo.MetaDescription;

            }
        }
        public void InitSeo(int eventId)
        {
            using (PyramidFinalContext context = new PyramidFinalContext())
            {
                var eventDb = context.Events.Find(eventId);
                if (eventDb != null)
                {
                    if (eventDb.Seo == null)
                    {
                        eventDb.Seo = new Seo();

                    }
                    eventDb.Seo.Alias = Tools.Transliteration.Translit(eventDb.Title);
                    eventDb.Seo.MetaTitle = eventDb.Title;
                    context.SaveChanges();

                }
            }

        }
        public override bool Delete(int id)
        {
            var data = _entities ?? new PyramidFinalContext();
            try
            {
                var objects = data.Set<Events>();
                var dbObject = GetDbObjectById(objects, id);
                if (dbObject == null)
                    return false;
                var seo = data.Seo.Find(dbObject.SeoId);
                if (seo != null)
                {
                    data.Seo.Remove(seo);
                }
                objects.Remove(dbObject);
                data.SaveChanges();
                return true;
            }
            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
        }

    }
}
