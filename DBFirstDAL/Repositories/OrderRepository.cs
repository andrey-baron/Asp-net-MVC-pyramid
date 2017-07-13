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
           
            dbEntity.TypeProgressOrder = (int)entity.TypeProgressOrder;
          
        }

        public override void UpdateAfterSaving(PyramidFinalContext dbContext, Orders dbEntity, Order entity, bool exists)
        {
            
           
        }

        public void Add(Order entity)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var dbEntity = new Orders() {
                    Adress = entity.Adress,
                    Email = entity.Email,
                    Phone = entity.Phone,
                    TypeProgressOrder = (int)entity.TypeProgressOrder,
                    UserName = entity.UserName,
                    
                };

               
                dbContext.Orders.Add(dbEntity);
                dbContext.SaveChanges();

                dbEntity.ProductOrders = entity.Products.Select(s => new ProductOrders()
                {
                    OrderId=dbEntity.Id,
                    ProductId=s.Product.Id,
                    Quantity=s.Quantity
                }).ToList();
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

        protected override IQueryable<Orders> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Orders> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        public override Order ConvertDbObjectToEntity(PyramidFinalContext context, Orders dbObject)
        {
            var order = new Order() {
                Products=dbObject.ProductOrders.Select(s=>new OrderProduct() {
                    Product=new ProductRepository().ConvertDbObjectToEntity(context,s.Products),
                    Quantity=s.Quantity                    
                }).ToList(),
                Adress=dbObject.Adress,
                Email=dbObject.Email,
                Id=dbObject.Id,
                Phone=dbObject.Phone,
                TypeProgressOrder= (Pyramid.Entity.Enumerable.TypeProgressOrder) dbObject.TypeProgressOrder,
                UserName=dbObject.UserName
            };
            return order;
        }

        protected override Orders GetDbObjectByEntity(DbSet<Orders> objects, Order entity)
        {
            return objects.FirstOrDefault(f => f.Id == entity.Id);
        }

        protected override Expression<Func<Orders, int>> GetIdByDbObjectExpression()
        {
            return r => r.Id;
        }
    }
}
