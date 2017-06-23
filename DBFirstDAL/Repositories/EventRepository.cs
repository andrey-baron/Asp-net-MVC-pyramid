using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DBFirstDAL.Repositories
{
   public class EventRepository: GenericRepository<PyramidFinalContext, Events>
    {
        public override IQueryable<Events> GetAll()
        {
            // говорим, что не надо создавать динамически генерируемые прокси-классы
            // (которые System.Data.Entity.DynamicProxies...)
            Context.Configuration.ProxyCreationEnabled = false;
            // отключаем ленивую загрузку
            Context.Configuration.LazyLoadingEnabled = false;
            var query = Context.Set<Events>().Include(i=>i.EventImages.Images).AsNoTracking();


            return query;
        }
        public override void AddOrUpdate(Events entity)
        {
            var efEvent = FindBy(f => f.Id == entity.Id).SingleOrDefault();
            if (efEvent==null)
            {
                var listProductIds = new List<int>(entity.Products.Select(i=>i.Id));
                entity.Products.Clear();
                foreach (var item in listProductIds)
                {
                    var efProd = Context.Products.Find(item);
                    entity.Products.Add(efProd);
                }
                Context.Events.Add(entity);
                Context.SaveChanges();


                entity.EventImages = new EventImages() {
                    ImageId = entity.EventImages.ImageId,
                    EventId=entity.Id
                };
               
                Context.SaveChanges();               
            }
            else
            {
                var test = Context.Entry(efEvent);
                Context.Entry(efEvent).CurrentValues.SetValues(entity);
                var listProductIds = new List<int>(entity.Products.Select(i => i.Id));

                efEvent.Products.Clear();
                foreach (var item in listProductIds)
                {
                    var efProd = Context.Products.Find(item);
                    efEvent.Products.Add(efProd);
                }
               
                Context.SaveChanges();


                entity.EventImages = new EventImages()
                {
                    ImageId = efEvent.EventImages.ImageId,
                    EventId = efEvent.Id
                };

                Context.SaveChanges();
            }
        }

        public bool DeleteReletedProduct(int id,int productId)
        {
            var actionResult = false;
            var efEvent = FindBy(i => i.Id == id).SingleOrDefault();
            if (efEvent!=null)
            {
                if (efEvent.Products != null)
                {
                    var efProd=efEvent.Products.FirstOrDefault(f => f.Id == productId);
                    if (efProd!=null)
                    {
                        efEvent.Products.Remove(efProd);
                        Save();
                        actionResult = true;

                    }
                }
            }
            return actionResult;
        }
    }
}
