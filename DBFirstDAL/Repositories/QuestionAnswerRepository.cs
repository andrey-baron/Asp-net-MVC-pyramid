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
    public class QuestionAnswerRepository : GenericRepository<QuestionAnswer, PyramidFinalContext, Pyramid.Entity.QuestionAnswer, SearchParamsBase, int>
    {
        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, QuestionAnswer dbEntity, Pyramid.Entity.QuestionAnswer entity, bool exists)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<QuestionAnswer> BuildDbObjectsList(PyramidFinalContext context, IQueryable<QuestionAnswer> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        protected override Pyramid.Entity.QuestionAnswer ConvertDbObjectToEntity(PyramidFinalContext context, QuestionAnswer dbObject)
        {
            throw new NotImplementedException();
        }

        protected override QuestionAnswer GetDbObjectByEntity(DbSet<QuestionAnswer> objects, Pyramid.Entity.QuestionAnswer entity)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<QuestionAnswer, int>> GetIdByDbObjectExpression()
        {
            throw new NotImplementedException();
        }
    }
}
