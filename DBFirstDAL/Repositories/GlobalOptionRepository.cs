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
            dbEntity.OptionContent = entity.OptionContent;
            if (!exists)
            {
                dbEntity.DescriptionKey = entity.Description;
                dbEntity.IsHtml = entity.IsHtml;
                dbEntity.StringKey = entity.StringKey;
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
