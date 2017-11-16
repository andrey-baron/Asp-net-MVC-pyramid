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
    public class FaqRepository : GenericRepository<Faq,PyramidFinalContext,Pyramid.Entity.FAQ, SearchParamsBase,int>
    {
        public FaqRepository(PyramidFinalContext context):base(context) { }
        public FaqRepository() { }
        public void AddEmptyQuestionAnswer(int faqId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efFaq = dbContext.Faq.Find(faqId);
                if (efFaq != null)
                {
                    efFaq.QuestionAnswer.Add(new QuestionAnswer()
                    {
                        Answer = "",
                        Question = ""
                    });
                }
                dbContext.SaveChanges();
            }
        }


        public  IEnumerable<FAQ> GetAllWithQuestionAnswer()
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                return dbContext.Faq.Include(i => i.QuestionAnswer).AsNoTracking().ToList().Select(s=>ConvertDbObjectToEntity(dbContext,s));
            }
           // return base.GetAll();
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Faq dbEntity, FAQ entity, bool exists)
        {
            dbEntity.Title = entity.Title;

        }
        public override void UpdateAfterSaving(PyramidFinalContext dbContext, Faq dbEntity, FAQ entity, bool exists)
        {
            
            dbEntity.QuestionAnswer.Clear();
            foreach (var item in entity.QuestionAnswer)
            {
                dbEntity.QuestionAnswer.Add(new QuestionAnswer() {
                    Answer=item.Answer,
                    Question=item.Question,
                    Id=item.Id
                });
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

        protected override IQueryable<Faq> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Faq> dbObjects, SearchParamsBase searchParams)
        {
            dbObjects = dbObjects.OrderBy(item => item.Id);
            return dbObjects;
        }

        public override FAQ ConvertDbObjectToEntity(PyramidFinalContext context, Faq dbObject)
        {
            var seo = new SeoRepository(context).Get(dbObject.SeoId.HasValue ? dbObject.SeoId.Value : 0);

            var faq = new FAQ() {
                Id=dbObject.Id,
               Title=dbObject.Title,
                FriendlyUrl = new RouteItemRepository(context).GetFriendlyUrl(dbObject.Id, Common.TypeEntityFromRouteEnum.Faq),
                Seo=seo,
                SeoId=dbObject.SeoId,

               QuestionAnswer = dbObject.QuestionAnswer.Select(s=>new Pyramid.Entity.QuestionAnswer()
               {
                   Answer=s.Answer,
                   Id=s.Id,
                   Question=s.Question
               }).ToList() 
            };
            return faq;
        }
        protected override FAQ ConvertDbObjectToEntityShort(PyramidFinalContext context, Faq dbObject)
        {
            var seo = new SeoRepository(context).Get(dbObject.SeoId.HasValue ? dbObject.SeoId.Value : 0);

            var faq = new FAQ()
            {
                Id = dbObject.Id,
                Title = dbObject.Title,
                FriendlyUrl = new RouteItemRepository(context).GetFriendlyUrl(dbObject.Id, Common.TypeEntityFromRouteEnum.Faq),
                Seo=seo,
                SeoId=dbObject.SeoId
            };
            return faq;
        }
        protected override Faq GetDbObjectByEntity(DbSet<Faq> objects, FAQ entity)
        {
            return objects.FirstOrDefault(i => i.Id == entity.Id);
        }

        protected override Expression<Func<Faq, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;
        }

        public override bool Delete(int id)
        {
            var data = _entities ?? new PyramidFinalContext();
            try
            {
                var objects = data.Set<Faq>();
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

        public void InitSeo(int fqId)
        {
            using (PyramidFinalContext context = new PyramidFinalContext())
            {
                var fqDb = context.Faq.Find(fqId);
                if (fqDb != null)
                {
                    if (fqDb.Seo == null)
                    {
                        fqDb.Seo = new Seo();

                    }
                    fqDb.Seo.Alias = Tools.Transliteration.Translit(fqDb.Title);
                    fqDb.Seo.MetaTitle = fqDb.Title;
                    context.SaveChanges();

                }
            }

        }
    }
}
