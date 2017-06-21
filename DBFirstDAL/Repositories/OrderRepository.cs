using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.Repositories
{
    public class OrderRepository : GenericRepository<PyramidFinalContext, Orders>
    {
        public override void AddOrUpdate(Orders entity)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var copyList = new List<DBFirstDAL.ProductOrders>(entity.ProductOrders);
                entity.ProductOrders.Clear();
                dbContext.Orders.Add(entity);
                dbContext.SaveChanges();
                int orderId = entity.Id;
                copyList = copyList.Select(i => new ProductOrders()
                {
                    OrderId = orderId,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                }).ToList();
                dbContext.ProductOrders.AddRange(copyList);
                dbContext.SaveChanges();

            }
        }
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
    }
}
