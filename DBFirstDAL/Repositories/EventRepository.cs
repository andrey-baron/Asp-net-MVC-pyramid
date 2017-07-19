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
        /*public override e<Events> GetAll()
        {
            // говорим, что не надо создавать динамически генерируемые прокси-классы
            // (которые System.Data.Entity.DynamicProxies...)
            Context.Configuration.ProxyCreationEnabled = false;
            // отключаем ленивую загрузку
            Context.Configuration.LazyLoadingEnabled = false;
            var query = Context.Set<Events>().Include(i=>i.EventImages.Images).AsNoTracking();


            return query;
        }*/
        //public override void AddOrUpdate(Events entity)
        //{
        //    var efEvent = FindBy(f => f.Id == entity.Id).SingleOrDefault();
        //    if (efEvent==null)
        //    {
        //        var listProductIds = new List<int>(entity.Products.Select(i=>i.Id));
        //        entity.Products.Clear();
        //        foreach (var item in listProductIds)
        //        {
        //            var efProd = Context.Products.Find(item);
        //            entity.Products.Add(efProd);
        //        }
        //        Context.Events.Add(entity);
        //        Context.SaveChanges();


        //        entity.EventImages = new EventImages() {
        //            ImageId = entity.EventImages.ImageId,
        //            EventId=entity.Id
        //        };
               
        //        Context.SaveChanges();               
        //    }
        //    else
        //    {
        //        var test = Context.Entry(efEvent);
        //        Context.Entry(efEvent).CurrentValues.SetValues(entity);
        //        var listProductIds = new List<int>(entity.Products.Select(i => i.Id));

        //        efEvent.Products.Clear();
        //        foreach (var item in listProductIds)
        //        {
        //            var efProd = Context.Products.Find(item);
        //            efEvent.Products.Add(efProd);
        //        }
               
        //        Context.SaveChanges();


        //        entity.EventImages = new EventImages()
        //        {
        //            ImageId = efEvent.EventImages.ImageId,
        //            EventId = efEvent.Id
        //        };

        //        Context.SaveChanges();
        //    }
        //}

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
            var entity = new Event() {
                Content= dbObject.Content,
                Id=dbObject.Id,
                DateEventEnd=dbObject.DateEventEnd,
                DateEventStart=dbObject.DateEventStart,
                //Image=dbObject.EventImages
                isActive=dbObject.isActive,
                Products=dbObject.Products.Select(s=>new ProductRepository().ConvertDbObjectToEntity(context,s)).ToList(),
                ShortContent=dbObject.ShortContent,
                Title= dbObject.Title
            };
            return entity;
        }

        protected override IQueryable<Events> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Events> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
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
           

        }
    }
}
