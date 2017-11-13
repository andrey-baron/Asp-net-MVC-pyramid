using Common.SearchClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data.Entity;
using System.Linq.Expressions;

namespace DBFirstDAL.Repositories
{
    public class RouteItemRepository : GenericRepository<RouteItem, PyramidFinalContext, Entity.RouteItem, SearchParamsRouteItem, int>
    {
        public RouteItemRepository(PyramidFinalContext context) : base(context) { }
        public RouteItemRepository(){ }

        public override void AddOrUpdate(Entity.RouteItem entity)
        {
            var data = _entities ?? new PyramidFinalContext();
            try
            {
                var objects = data.Set<RouteItem>();
                var dbObject = GetDbObjectByEntity(objects, entity);
                string friendlyUrl = null;
                bool exists = dbObject != null;
                if (!exists)
                {
                    dbObject = new RouteItem();
                }
                else
                {
                    friendlyUrl = dbObject.FriendlyUrl;
                }
                UpdateBeforeSaving(data, dbObject, entity, exists);
                if (!exists)
                {
                    objects.Add(dbObject);
                }
               
                data.SaveChanges();
                ReBuildRelatedFriendlyUrl(data, friendlyUrl);
                UpdateAfterSaving(data, dbObject, entity, exists);
                data.SaveChanges();

            }
            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
        }
        public override Entity.RouteItem ConvertDbObjectToEntity(PyramidFinalContext context, RouteItem dbObject)
        {
            return new Entity.RouteItem(dbObject.Id,dbObject.FriendlyUrl,dbObject.ControllerName,dbObject.ActionName,(int)dbObject.ContentId);
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, RouteItem dbEntity, Entity.RouteItem entity, bool exists)
        {
            //dbEntity.Id = entity.Id;
            dbEntity.TypeEntity = (int)entity.Type;
            dbEntity.ControllerName = entity.ControllerName;
            dbEntity.ActionName = entity.ActionName;
            dbEntity.ContentId = entity.ContentId;
            switch (entity.Type)
            {
                case Common.TypeEntityFromRouteEnum.ProductType:
                    var friendlyUrlProduct=new ProductRepository(dbContext).GetChainFrendlyUrlByAlias(entity.ContentId);
                    if (friendlyUrlProduct!=null)
                    {
                        dbEntity.FriendlyUrl = friendlyUrlProduct;
                    }
                    break;
                case Common.TypeEntityFromRouteEnum.CategoryType:
                    var friendlyUrlCategory = new CategoryRepository(dbContext).GetChainFrendlyUrlByAlias(entity.ContentId);
                    if (friendlyUrlCategory != null)
                    {
                        dbEntity.FriendlyUrl = friendlyUrlCategory;
                    }
                    break;
                case Common.TypeEntityFromRouteEnum.PageType:
                    var page = new PageRepository(dbContext).Get(entity.ContentId);
                    if (page!=null&& page.Seo!=null)
                    {
                        dbEntity.FriendlyUrl = string.Format("/{0}", page.Seo.Alias.Replace("/",""));

                    }
                    break;
                default:
                    break;
            }
            if (dbEntity.FriendlyUrl==null)
            {
                dbEntity.FriendlyUrl = string.Format("/{0}/{1}/{2}", dbEntity.ControllerName, dbEntity.ActionName, dbEntity.ContentId);
            }
        }

        //public override void UpdateAfterSaving(PyramidFinalContext dbContext, RouteItem dbEntity, Entity.RouteItem entity, bool exists)
        //{
        //    ReBuildRelatedFriendlyUrl(dbContext, dbEntity.FriendlyUrl);
        //}
        protected override IQueryable<RouteItem> BuildDbObjectsList(PyramidFinalContext context, IQueryable<RouteItem> dbObjects, SearchParamsRouteItem searchParams)
        {
            return dbObjects;
        }

        protected override RouteItem GetDbObjectByEntity(DbSet<RouteItem> objects, Entity.RouteItem entity)
        {
            if (entity.ContentId!=0)
            {
                return objects.FirstOrDefault(f => f.ControllerName.ToLower() == entity.ControllerName.ToLower() &&
             f.ActionName.ToLower() == entity.ActionName.ToLower() &&
             f.ContentId == entity.ContentId);

            }
            else
            {
                return objects.FirstOrDefault(f => f.ControllerName.ToLower() == entity.ControllerName.ToLower() &&
             f.ActionName.ToLower() == entity.ActionName.ToLower() &&
             f.ContentId ==null);
            }
            
        }

        protected override Expression<Func<RouteItem, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;
        }
        public Entity.RouteItem Get(string friendlyUrl)
        {
            var data = _entities ?? new PyramidFinalContext();
            try
            {
                var routeItemDal= data.RouteItems.FirstOrDefault(f => f.FriendlyUrl.ToLower() == friendlyUrl.ToLower());
                return routeItemDal != null ? ConvertDbObjectToEntity(data, routeItemDal) : null;
            }
            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
        }

        public Entity.RouteItem Get(string controllerName, string actionName,int contentId) {
            using (PyramidFinalContext data= new PyramidFinalContext())
            {
                var item = data.RouteItems.FirstOrDefault(f => f.ControllerName.ToLower() == controllerName.ToLower() &&
                 f.ActionName.ToLower() == actionName.ToLower() &&
                 f.ContentId == contentId);
                return item != null ? ConvertDbObjectToEntity(data,item) : null;
            }
        }

        protected void ReBuildRelatedFriendlyUrl(PyramidFinalContext dbContext, string url) {
            //using (PyramidFinalContext data = new PyramidFinalContext())
            //{
            if (!string.IsNullOrEmpty(url))
            {
                var objects = dbContext.RouteItems.Where(w => w.FriendlyUrl.Contains(url));
                foreach (var item in objects)
                {
                    switch ((Common.TypeEntityFromRouteEnum)item.TypeEntity)
                    {
                        case Common.TypeEntityFromRouteEnum.ProductType:
                            var friendlyUrlProduct = new ProductRepository(dbContext).GetChainFrendlyUrlByAlias(item.ContentId.Value);
                            if (friendlyUrlProduct != null)
                            {
                                item.FriendlyUrl = friendlyUrlProduct;
                            }
                            break;
                        case Common.TypeEntityFromRouteEnum.CategoryType:
                            var friendlyUrlCategory = new CategoryRepository(dbContext).GetChainFrendlyUrlByAlias(item.ContentId.Value);
                            if (friendlyUrlCategory != null)
                            {
                                item.FriendlyUrl = friendlyUrlCategory;
                            }
                            break;

                        case Common.TypeEntityFromRouteEnum.PageType:
                            break;
                        default:
                            break;
                    }
                }
            }
            //}
        }

        public string GetFriendlyUrl(int contentId , Common.TypeEntityFromRouteEnum typeEntity)
        {
            var context = _entities ?? new PyramidFinalContext();
            try
            {
                var route=context.RouteItems.FirstOrDefault(f => f.TypeEntity == (int)typeEntity && f.ContentId == contentId);
                if (route!=null)
                {
                    return route.FriendlyUrl;
                }
                return null;
            }
            finally
            {
                if (_entities==null)
                {
                    context.Dispose();
                }
            }
        }
    }
}
