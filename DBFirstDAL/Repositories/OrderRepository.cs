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
    public class OrderRepository : GenericRepository<Orders,PyramidFinalContext,Pyramid.Entity.Order, SearchParamsBase,int>
    {
        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Orders dbEntity, Order entity, bool exists)
        {
            throw new NotImplementedException();
        }

        //public override void AddOrUpdate(Orders entity)
        //{
        //    using (PyramidFinalContext dbContext = new PyramidFinalContext())
        //    {
        //        var copyList = new List<DBFirstDAL.ProductOrders>(entity.ProductOrders);
        //        entity.ProductOrders.Clear();
        //        dbContext.Orders.Add(entity);
        //        dbContext.SaveChanges();
        //        int orderId = entity.Id;
        //        copyList = copyList.Select(i => new ProductOrders()
        //        {
        //            OrderId = orderId,
        //            ProductId = i.ProductId,
        //            Quantity = i.Quantity,
        //        }).ToList();
        //        dbContext.ProductOrders.AddRange(copyList);
        //        dbContext.SaveChanges();

        //    }
        //}
        public void UpdateType(int orderId, int typeOrder)
        {
            using (PyramidFinalContext dbcontext= new PyramidFinalContext())
            {
                var efOrder = dbcontext.Orders.FirstOrDefault(i => i.Id == orderId);
                if (efOrder!=null)
                {
                    efOrder.TypeProgressOrder = typeOrder;
                    dbcontext.SaveChanges();
                }
            }
        }

        protected override IQueryable<Orders> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Orders> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        protected override Order ConvertDbObjectToEntity(PyramidFinalContext context, Orders dbObject)
        {
            throw new NotImplementedException();
        }

        protected override Orders GetDbObjectByEntity(DbSet<Orders> objects, Order entity)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Orders, int>> GetIdByDbObjectExpression()
        {
            throw new NotImplementedException();
        }
    }
}
