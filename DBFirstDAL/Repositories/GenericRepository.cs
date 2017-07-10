using DBFirstDAL.Intercaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.SearchClasses;
using System.Linq.Expressions;
using System.Data.Entity;

namespace DBFirstDAL.Repositories
{
    public abstract class GenericRepository<TDbObject, TDbContext, TEntity, TSearchParams, TObjectId> :
    IGenericRepository<TDbObject, TDbContext, TEntity, TSearchParams, TObjectId>
    where TDbObject : class, new()
    where TDbContext : PyramidFinalContext, new()
    where TEntity : class
    where TSearchParams : SearchParamsBase
    {

        protected readonly TDbContext _entities;

        public TDbContext Context
        {

            get { return _entities; }
        }
        protected GenericRepository(TDbContext context)
        {
            _entities = context;
        }
        protected GenericRepository() { }

        public virtual void UpdateAfterSaving(TDbContext dbContext, TDbObject dbEntity, TEntity entity, bool exists)
        {
            
        }


        public abstract void UpdateBeforeSaving(TDbContext dbContext, TDbObject dbEntity, TEntity entity, bool exists);

        public virtual SearchResult<TEntity> Get(TSearchParams searchParams)
        {
            var data = _entities ?? new TDbContext();
            try
            {
                var objects = data.Set<TDbObject>().AsQueryable();
                objects = BuildDbObjectsList(data, objects, searchParams);
                var visitor = new OrderedQueryableVisitor();
                visitor.Visit(objects.Expression);
                objects = visitor.IsOrdered
                    ? (objects as IOrderedQueryable<TDbObject>).ThenBy(GetIdByDbObjectExpression())
                    : objects.OrderBy(GetIdByDbObjectExpression());
                var result = new SearchResult<TEntity>
                {
                    Total = objects.Count(),
                    RequestedObjectsCount = searchParams.ObjectsCount,
                    RequestedStartIndex = searchParams.StartIndex
                };
                objects = objects.Skip(searchParams.StartIndex);
                if (searchParams.ObjectsCount != null)
                    objects = objects.Take(searchParams.ObjectsCount.Value);
                result.Objects = objects.ToList().Select(item => ConvertDbObjectToEntityShort(data, item)).ToList();
                return result;
            }
            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
        }

        public virtual TEntity Get(TObjectId id)
        {
            var data = _entities ?? new TDbContext();
            try
            {
                var dbObject = GetDbObjectById(data.Set<TDbObject>(), id);
                return dbObject != null ? ConvertDbObjectToEntity(data, dbObject) : null;
            }
            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
        }

        public virtual IEnumerable<TDbObject> GetAll()
        {
            var data = _entities ?? new TDbContext();
            // говорим, что не надо создавать динамически генерируемые прокси-классы
            // (которые System.Data.Entity.DynamicProxies...)
            //data.Configuration.ProxyCreationEnabled = false;
            // отключаем ленивую загрузку
            //_entities.Configuration.LazyLoadingEnabled = false;
            try
            {
                IQueryable<TDbObject> query = data.Set<TDbObject>().AsNoTracking();
                return query.ToList();
            }

            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
           
        }
       
        public IEnumerable<TDbObject> FindBy(System.Linq.Expressions.Expression<Func<TDbObject, bool>> predicate)
        {
            var data = _entities ?? new TDbContext();
            
            try
            {
                IQueryable<TDbObject> query = data.Set<TDbObject>().Where(predicate);
                return query.ToList();
            }

            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
           
        }
      
        public virtual bool Delete(TObjectId id)
        {
            var data = _entities ?? new TDbContext();
            try
            {
                var objects = data.Set<TDbObject>();
                var dbObject = GetDbObjectById(objects, id);
                if (dbObject == null)
                    return false;
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

        //public virtual void Edit(TDbObject entity)
        //{
        //        _entities.Entry(entity).CurrentValues.SetValues(entity);
        //        _entities.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        //}

        public virtual void AddOrUpdate(TEntity entity)
        {
           //var prop= entity.GetType().GetProperty("Id");
           // int id = (int)prop.GetValue(entity, null);

           // var efEntity = _entities.Set<TDbObject>().Find(id);
           // if (efEntity == null)
           // {
           //     _entities.Set<TDbObject>().Add(entity);
           // }
           // else
           // {
           //     _entities.Entry(efEntity).CurrentValues.SetValues(entity);
           //     _entities.Entry(efEntity).State = System.Data.Entity.EntityState.Modified;

           // }

            var data = _entities ?? new TDbContext();
            try
            {
                var objects = data.Set<TDbObject>();
                var dbObject = GetDbObjectByEntity(objects, entity);
                bool exists = dbObject != null;
                if (!exists)
                {
                    dbObject = new TDbObject();
                }
                UpdateBeforeSaving(data, dbObject, entity, exists);
                if (!exists)
                {
                    objects.Add(dbObject);
                }
                data.SaveChanges();
                UpdateAfterSaving(data, dbObject, entity, exists);
                data.SaveChanges();
                
            }
            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
        }

        //public virtual void Save()
        //{
        //    _entities.SaveChanges();
        //}

        protected virtual TDbObject GetDbObjectById(DbSet<TDbObject> objects, TObjectId objectId) {
            return objects.Find(objectId);
        }

        protected abstract TDbObject GetDbObjectByEntity(DbSet<TDbObject> objects, TEntity entity);

        protected abstract Expression<Func<TDbObject, int>> GetIdByDbObjectExpression();

        protected abstract TEntity ConvertDbObjectToEntity(TDbContext context, TDbObject dbObject);

        protected virtual TEntity ConvertDbObjectToEntityShort(TDbContext context, TDbObject dbObject)
        {
            return ConvertDbObjectToEntity(context, dbObject);
        }

        protected abstract IQueryable<TDbObject> BuildDbObjectsList(TDbContext context, IQueryable<TDbObject> dbObjects, TSearchParams searchParams);

    }
}
