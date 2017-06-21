using DBFirstDAL.Intercaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.Repositories
{
    public abstract class GenericRepository<C, T> :
    IGenericRepository<T> where T : class where C : PyramidFinalContext, new()
    {

        private C _entities = new C();
       
        public C Context
        {

            get { return _entities; }
            set { _entities = value; }
        }

        public virtual IQueryable<T> GetAll()
        {
            // говорим, что не надо создавать динамически генерируемые прокси-классы
            // (которые System.Data.Entity.DynamicProxies...)
            _entities.Configuration.ProxyCreationEnabled = false;
            // отключаем ленивую загрузку
            //_entities.Configuration.LazyLoadingEnabled = false;
            IQueryable<T> query = _entities.Set<T>().AsNoTracking();
            return query;
        }
       
        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
         
            IQueryable<T> query = _entities.Set<T>().Where(predicate);
            return query;
        }

        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
         
                _entities.Entry(entity).CurrentValues.SetValues(entity);
                _entities.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        
            
        }

        public virtual void AddOrUpdate(T entity)
        {
           var prop= entity.GetType().GetProperty("Id");
            int id = (int)prop.GetValue(entity, null);

            var efEntity = _entities.Set<T>().Find(id);
            if (efEntity == null)
            {
                _entities.Set<T>().Add(entity);
            }
            else
            {
                _entities.Entry(efEntity).CurrentValues.SetValues(entity);
                _entities.Entry(efEntity).State = System.Data.Entity.EntityState.Modified;

            }

        }


        public virtual void Save()
        {
            _entities.SaveChanges();
        }

        protected virtual void UpdateFieldsBeforeSave()
        {
            
        }

        protected virtual void UpdateFieldsAfterSave()
        {
        }
    }
}
