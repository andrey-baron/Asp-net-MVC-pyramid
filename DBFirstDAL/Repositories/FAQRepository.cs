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
            var efFaq = FindBy(i => i.Id == faqId).SingleOrDefault();
            if (efFaq!=null)
            {
                efFaq.QuestionAnswer.Add(new QuestionAnswer()
                {
                    Answer = "",
                    Question = ""
                });
            }
            Context.SaveChanges();
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

        public  IEnumerable<Faq> GetAllWithQuestionAnswer()
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                return dbContext.Faq.Include(i => i.QuestionAnswer).AsNoTracking().ToList();
            }
           // return base.GetAll();
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Faq dbEntity, FAQ entity, bool exists)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<Faq> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Faq> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        protected override FAQ ConvertDbObjectToEntity(PyramidFinalContext context, Faq dbObject)
        {
            throw new NotImplementedException();
        }

        protected override Faq GetDbObjectByEntity(DbSet<Faq> objects, FAQ entity)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Faq, int>> GetIdByDbObjectExpression()
        {
            throw new NotImplementedException();
        }
    }
}
