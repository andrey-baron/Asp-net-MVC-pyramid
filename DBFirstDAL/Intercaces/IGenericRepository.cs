using Common.SearchClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.Intercaces
{
    public interface IGenericRepository<TDbObject, TDbContext, TEntity, TSearchParams, TObjectId>
        //where TdbObject : class
        //where TdbContext:PyramidFinalContext
        //where TEntity:class
        //where TSearchParams:SearchParamsBase
    {

        //IQueryable<TdbObject> GetAll();
        IEnumerable<TDbObject> FindBy(Expression<Func<TDbObject, bool>> predicate);
        //void Add(TDbObject dbEntity);
        bool Delete(TObjectId id);
        //void Edit(TdbObject dbEntity);
        void AddOrUpdate(TEntity dbEntity);

       // void Save();

        void UpdateBeforeSaving(TDbContext dbContext, TDbObject dbEntity, TEntity entity,bool exists);

        void UpdateAfterSaving(TDbContext dbContext, TDbObject dbEntity, TEntity entity, bool exists);

        SearchResult<TEntity> Get(TSearchParams searchParams);
    }
}
