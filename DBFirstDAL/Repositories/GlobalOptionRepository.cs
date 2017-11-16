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
    public class GlobalOptionRepository : GenericRepository<GlobalOption, PyramidFinalContext, Pyramid.Entity.GlobalOptionEntity, SearchParamsBase, int>
    {

        //public override void AddOrUpdate(GlobalOptionEntity entity)
        //{
        //    var data = _entities ?? new PyramidFinalContext();
        //    try
        //    {
        //        var objects = data.Set<GlobalOption>();
        //        var dbObject = GetDbObjectByEntity(objects, entity);
        //        bool exists = dbObject != null;
        //        if (!exists)
        //        {
        //            dbObject = new GlobalOption();
        //        }
        //        string urlForEntities = "";
                
        //        UpdateBeforeSaving(data, dbObject, entity, exists);
        //        if (!exists)
        //        {
        //            objects.Add(dbObject);
        //        }
        //        data.SaveChanges();
            
        //        UpdateAfterSaving(data, dbObject, entity, exists);
        //        data.SaveChanges();

        //    }
        //    finally
        //    {
        //        if (_entities == null)
        //            data.Dispose();
        //    }
        //}
        public GlobalOptionEntity Get(string key)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var dbObj=dbContext.GlobalOption.FirstOrDefault(f => f.StringKey == key);
               return ConvertDbObjectToEntity(dbContext,dbObj);
            }
        }
        public override GlobalOptionEntity ConvertDbObjectToEntity(PyramidFinalContext context, GlobalOption dbObject)
        {
            var entity = new GlobalOptionEntity()
            {
                Id = dbObject.Id,
                OptionContent=dbObject.OptionContent,
                StringKey=dbObject.StringKey,
                Description=dbObject.DescriptionKey,
                IsHtml=dbObject.IsHtml
            };
            return entity;
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, GlobalOption dbEntity, GlobalOptionEntity entity, bool exists)
        {
            if (dbEntity.StringKey==Common.Constant.KeyEvent||
                dbEntity.StringKey == Common.Constant.KeyFaq||
                dbEntity.StringKey==Common.Constant.KeyRecommendation)
            {
                dbEntity.OptionContent = entity.OptionContent.Replace("/","");
            }
            else
            {
                dbEntity.OptionContent = entity.OptionContent;
            }
            if (!exists)
            {
                dbEntity.DescriptionKey = entity.Description;
                dbEntity.IsHtml = entity.IsHtml;
                dbEntity.StringKey = entity.StringKey;
            }
        }
        public override void UpdateAfterSaving(PyramidFinalContext dbContext, GlobalOption dbEntity, GlobalOptionEntity entity, bool exists)
        {
            switch (dbEntity.StringKey)
            {
                case Common.Constant.KeyEvent:
                    new RouteItemRepository(dbContext).AddOrUpdate(new Entity.RouteItem() {
                        ActionName=Common.Constant.ValEventAction,
                        ControllerName=Common.Constant.ValEventController,
                        Type=Common.TypeEntityFromRouteEnum.Event
                    });

                    break;
                case Common.Constant.KeyRecommendation:
                    new RouteItemRepository(dbContext).AddOrUpdate(new Entity.RouteItem()
                    {
                        ActionName = Common.Constant.ValRecommendationAction,
                        ControllerName = Common.Constant.ValRecommendationController,
                        Type = Common.TypeEntityFromRouteEnum.RecommendationType
                    });

                    break;
                case Common.Constant.KeyFaq:

                    new RouteItemRepository(dbContext).AddOrUpdate(new Entity.RouteItem()
                    {
                        ActionName = Common.Constant.ValFaqAction,
                        ControllerName = Common.Constant.ValFaqController,
                        Type = Common.TypeEntityFromRouteEnum.Faq
                    });
                    break;
                default:
                    break;
            }
        }

        protected override IQueryable<GlobalOption> BuildDbObjectsList(PyramidFinalContext context, IQueryable<GlobalOption> dbObjects, SearchParamsBase searchParams)
        {
            return dbObjects;
        }

        protected override GlobalOption GetDbObjectByEntity(DbSet<GlobalOption> objects, GlobalOptionEntity entity)
        {
            return objects.FirstOrDefault(f => f.Id == entity.Id);
        }


        protected override Expression<Func<GlobalOption, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;

        }
        
        public bool isExist(string key)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                return dbContext.GlobalOption.FirstOrDefault(i => i.StringKey == key) != null;
            }
        }
    }
}
