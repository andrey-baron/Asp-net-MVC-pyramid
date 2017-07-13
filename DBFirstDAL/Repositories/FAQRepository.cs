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

        //public override void AddOrUpdate(Faq entity)
        //{
        //    var prop = entity.GetType().GetProperty("Id");
        //    int id = (int)prop.GetValue(entity, null);
        //    using (PyramidFinalContext dbContext= new PyramidFinalContext())
        //    {
        //        var efEntity = dbContext.Faq.Find(id);

        //        if (efEntity == null)
        //        {
        //            bool flagQA = false;
        //            List<QuestionAnswer> tempQA = new List<QuestionAnswer>();;
        //            if (entity.QuestionAnswer != null)
        //            {
        //                flagQA = true;
        //                 tempQA = new List<QuestionAnswer>(entity.QuestionAnswer);
        //                entity.QuestionAnswer.Clear();
        //            }
        //            dbContext.Faq.Add(entity);
        //            dbContext.Entry(entity).State = System.Data.Entity.EntityState.Added;
        //            dbContext.SaveChanges();
        //            if (flagQA)
        //            {
        //                entity.QuestionAnswer = tempQA;
        //            }
        //            dbContext.SaveChanges();
        //        }
        //        else
        //        {
        //            dbContext.Entry(efEntity).CurrentValues.SetValues(entity);
        //            efEntity.QuestionAnswer.Clear();
        //            efEntity.QuestionAnswer = entity.QuestionAnswer;

        //            dbContext.SaveChanges();

        //        }
        //    }


        //}

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
        }

        protected override IQueryable<Faq> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Faq> dbObjects, SearchParamsBase searchParams)
        {
            dbObjects = dbObjects.OrderBy(item => item.Id);
            return dbObjects;
        }

        public override FAQ ConvertDbObjectToEntity(PyramidFinalContext context, Faq dbObject)
        {
            var faq = new FAQ() {
                Id=dbObject.Id,
               Title=dbObject.Title,
               QuestionAnswer=dbObject.QuestionAnswer.Select(s=>new Pyramid.Entity.QuestionAnswer()
               {
                   Answer=s.Answer,
                   Id=s.Id,
                   Question=s.Question
               }).ToList() 
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
    }
}
