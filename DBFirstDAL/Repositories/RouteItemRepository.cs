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
                    var obj = data.RouteItems.FirstOrDefault(f => f.FriendlyUrl == dbObject.FriendlyUrl);
                    if (obj!=null)
                    {
                        data.RouteItems.Remove(obj);
                        data.SaveChanges();
                    }
                    objects.Add(dbObject);
                }
               
                data.SaveChanges();
                ReBuildRelatedFriendlyUrl(data, friendlyUrl);
                UpdateAfterSaving(data, dbObject, entity, exists);
                data.SaveChanges();

            }
            catch( Exception ex)
            {

            }
            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
        }
        public override Entity.RouteItem ConvertDbObjectToEntity(PyramidFinalContext context, RouteItem dbObject)
        {
            return new Entity.RouteItem(dbObject.Id,dbObject.FriendlyUrl,dbObject.ControllerName,dbObject.ActionName,dbObject.ContentId.HasValue?dbObject.ContentId.Value:0);
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, RouteItem dbEntity, Entity.RouteItem entity, bool exists)
        {
            //dbEntity.Id = entity.Id;
            dbEntity.TypeEntity = (int)entity.Type;
            dbEntity.ControllerName = entity.ControllerName;
            dbEntity.ActionName = entity.ActionName;
            dbEntity.ContentId = entity.ContentId==0?(int?)null:entity.ContentId;
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
                case Common.TypeEntityFromRouteEnum.Faq:
                    var fq = new FaqRepository(dbContext).Get(entity.ContentId);
                    var globPrefixFaq = new GlobalOptionRepository().Get(Common.Constant.KeyFaq);
                    if (fq != null && fq.Seo != null)
                    {
                        dbEntity.FriendlyUrl = string.Format("/{0}/{1}", globPrefixFaq.OptionContent, fq.Seo.Alias.Replace("/", ""));

                    }
                    else
                    {
                        if (dbEntity.ActionName == Common.Constant.ValFaqAction &&
                            dbEntity.ControllerName == Common.Constant.ValFaqController &&
                            dbEntity.ContentId == null)
                        {
                            dbEntity.FriendlyUrl = string.Format("/{0}", globPrefixFaq.OptionContent);

                        }
                    }
                    break;
                case Common.TypeEntityFromRouteEnum.Event:
                    var evnt = new EventRepository(dbContext).Get(entity.ContentId);
                    var globPrefixEvent = new GlobalOptionRepository().Get(Common.Constant.KeyEvent);
                    if (evnt != null && evnt.Seo != null)
                    {
                        dbEntity.FriendlyUrl = string.Format("/{0}/{1}", globPrefixEvent.OptionContent, evnt.Seo.Alias.Replace("/", ""));

                    }
                    else
                    {
                        if (dbEntity.ActionName==Common.Constant.ValEventAction && 
                            dbEntity.ControllerName==Common.Constant.ValEventController&&
                            dbEntity.ContentId==null)
                        {
                            dbEntity.FriendlyUrl = string.Format("/{0}", globPrefixEvent.OptionContent);

                        }
                    }
                    break;
                case Common.TypeEntityFromRouteEnum.RecommendationType:
                    var recommend = new RecommendationRepository(dbContext).Get(entity.ContentId);
                    var globPrefixRecommend = new GlobalOptionRepository().Get(Common.Constant.KeyRecommendation);
                    if (recommend != null && recommend.Seo != null)
                    {
                        dbEntity.FriendlyUrl = string.Format("/{0}/{1}", globPrefixRecommend.OptionContent, recommend.Seo.Alias.Replace("/", ""));

                    }
                    else
                    {
                        if (dbEntity.ActionName == Common.Constant.ValRecommendationAction &&
                            dbEntity.ControllerName == Common.Constant.ValRecommendationController &&
                            dbEntity.ContentId == null)
                        {
                            dbEntity.FriendlyUrl = string.Format("/{0}", globPrefixRecommend.OptionContent);

                        }
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

        public Entity.RouteItem Get(string controllerName, string actionName,int? contentId) {
            var data = _entities ?? new PyramidFinalContext();
            try
            {
                RouteItem item=null;
                if (contentId.HasValue&& contentId.Value!=0)
                {
                    item = data.RouteItems .FirstOrDefault(f => f.ControllerName.ToLower() == controllerName.ToLower() &&
                 f.ActionName.ToLower() == actionName.ToLower() &&
                 f.ContentId == contentId.Value);
                }
                else
                {
                    item = data.RouteItems.FirstOrDefault(f => f.ControllerName.ToLower() == controllerName.ToLower() &&
                f.ActionName.ToLower() == actionName.ToLower() &&
                f.ContentId == null);
                }
                return item != null ? ConvertDbObjectToEntity(data, item) : null;
            }
            finally
            {
                if (_entities == null)
                    data.Dispose();
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
                            var friendlyUrlProduct = new ProductRepository(dbContext).GetChainFrendlyUrlByAlias(item.ContentId.HasValue ? item.ContentId.Value : 0);
                            if (friendlyUrlProduct != null)
                            {
                                item.FriendlyUrl = friendlyUrlProduct;
                            }
                            break;
                        case Common.TypeEntityFromRouteEnum.CategoryType:
                            var friendlyUrlCategory = new CategoryRepository(dbContext).GetChainFrendlyUrlByAlias(item.ContentId.HasValue ? item.ContentId.Value : 0);
                            if (friendlyUrlCategory != null)
                            {
                                item.FriendlyUrl = friendlyUrlCategory;
                            }
                            break;

                        case Common.TypeEntityFromRouteEnum.PageType:
                            break;
                        case Common.TypeEntityFromRouteEnum.Faq:
                            var fq = new FaqRepository(dbContext).Get(item.ContentId.HasValue? item.ContentId.Value:0);
                            var globPrefixFaq = new GlobalOptionRepository().Get(Common.Constant.KeyFaq);
                            if (fq != null && fq.Seo != null)
                            {
                                item.FriendlyUrl = string.Format("/{0}/{1}", globPrefixFaq.OptionContent, fq.Seo.Alias.Replace("/", ""));

                            }
                            break;
                        case Common.TypeEntityFromRouteEnum.Event:
                            var evnt = new EventRepository(dbContext).Get(item.ContentId.HasValue ? item.ContentId.Value : 0);
                            var globPrefixEvent = new GlobalOptionRepository().Get(Common.Constant.KeyEvent);
                            if (evnt != null && evnt.Seo != null)
                            {
                                item.FriendlyUrl = string.Format("/{0}/{1}", globPrefixEvent.OptionContent, evnt.Seo.Alias.Replace("/", ""));

                            }
                            break;
                        case Common.TypeEntityFromRouteEnum.RecommendationType:
                            var recommend = new RecommendationRepository(dbContext).Get(item.ContentId.HasValue ? item.ContentId.Value : 0);
                            var globPrefixRecommend = new GlobalOptionRepository().Get(Common.Constant.KeyRecommendation);
                            if (recommend != null && recommend.Seo != null)
                            {
                                item.FriendlyUrl = string.Format("/{0}/{1}", globPrefixRecommend.OptionContent, recommend.Seo.Alias.Replace("/", ""));

                            }
                            
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

       // public bool Exist()
    }
}
