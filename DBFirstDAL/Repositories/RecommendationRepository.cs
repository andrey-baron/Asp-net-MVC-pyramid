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
        public RecommendationRepository(PyramidFinalContext context) : base(context) { }

        public RecommendationRepository() { }

        public override Recommendation ConvertDbObjectToEntity(PyramidFinalContext context, Recommendations dbObject)
        {
            var seo = new SeoRepository(context).Get(dbObject.SeoId.HasValue ? dbObject.SeoId.Value : 0);

            var recommend = new Recommendation() {
                Id=dbObject.Id,
                Content=dbObject.Content,
                ShortContent=dbObject.ShortContent,
                Title=dbObject.Title,
                Image= Convert.ConvertImageToEntity.Convert(dbObject.Images.FirstOrDefault()),
                FriendlyUrl=new RouteItemRepository(context).GetFriendlyUrl(dbObject.Id,Common.TypeEntityFromRouteEnum.RecommendationType),
                Seo=seo,
                SeoId=dbObject.SeoId
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
                Image = Convert.ConvertImageToEntity.Convert(dbObject.Images.FirstOrDefault()),
                FriendlyUrl = new RouteItemRepository(context).GetFriendlyUrl(dbObject.Id, Common.TypeEntityFromRouteEnum.RecommendationType),

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
        public void InitSeo(int recomId)
        {
            using (PyramidFinalContext context = new PyramidFinalContext())
            {
                var recommendDb = context.Recommendations.Find(recomId);
                if (recommendDb != null)
                {
                    if (recommendDb.Seo == null)
                    {
                        recommendDb.Seo = new Seo();

                    }
                    recommendDb.Seo.Alias = Tools.Transliteration.Translit(recommendDb.Title);
                    recommendDb.Seo.MetaTitle = recommendDb.Title;
                    context.SaveChanges();

                }
            }

        }
        public override bool Delete(int id)
        {
            var data = _entities ?? new PyramidFinalContext();
            try
            {
                var objects = data.Set<Recommendations>();
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
