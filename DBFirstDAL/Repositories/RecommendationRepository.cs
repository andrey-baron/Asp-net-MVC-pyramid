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
    public class RecommendationRepository : GenericRepository<Recommendations, PyramidFinalContext, Pyramid.Entity.Recommendation, SearchParamsBase, int>
    {
        public override Recommendation ConvertDbObjectToEntity(PyramidFinalContext context, Recommendations dbObject)
        {
            var recommend = new Recommendation() {
                Id=dbObject.Id,
                Content=dbObject.Content,
                ShortContent=dbObject.ShortContent,
                Title=dbObject.Title,
                Image= Convert.ConvertImageToEntity.Convert(dbObject.Images.FirstOrDefault())
            };
            return recommend;
        }
        protected override Recommendation ConvertDbObjectToEntityShort(PyramidFinalContext context, Recommendations dbObject)
        {
            var recommend = new Recommendation()
            {
                Id = dbObject.Id,
                Title = dbObject.Title,
                ShortContent=dbObject.ShortContent,
                Image = Convert.ConvertImageToEntity.Convert(dbObject.Images.FirstOrDefault())
            };
            return recommend;
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Recommendations dbEntity, Recommendation entity, bool exists)
        {
            dbEntity.Title = entity.Title;
            dbEntity.Content = entity.Content;
            dbEntity.ShortContent = entity.ShortContent;

           
        }
        public override void UpdateAfterSaving(PyramidFinalContext dbContext, Recommendations dbEntity, Recommendation entity, bool exists)
        {
            var dbImg=dbEntity.Images.FirstOrDefault(f => f.Id == entity.Image.Id);
            if (dbImg==null)
            {
                
                var newImg=dbContext.Images.Find(entity.Image.Id);
                if (newImg!=null)
                {
                    dbEntity.Images.Clear();
                    dbEntity.Images.Add(newImg);
                    dbContext.SaveChanges();

                }
               
            }
        }

        protected override IQueryable<Recommendations> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Recommendations> dbObjects, SearchParamsBase searchParams)
        {
            return dbObjects.OrderBy(i => i.Id);
        }

        protected override Recommendations GetDbObjectByEntity(DbSet<Recommendations> objects, Recommendation entity)
        {
            return objects.FirstOrDefault(f => f.Id == entity.Id);
        }

        protected override Expression<Func<Recommendations, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;
        }
    }
}
