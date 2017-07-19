using AutoMapper;
using DBFirstDAL.Repositories;
using PagedList;
using Pyramid.Global;
using Pyramid.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private OrderRepository _orederRepository;

        public OrderController()
        {
            _orederRepository = new OrderRepository();
        }
        public ActionResult Index(int? page)
        {
            var orders = _orederRepository.GetAll().ToList();
           
            int pageNumber = (page ?? 1);
            var modelList = new PagedList<Entity.Order>(
            orders,
            pageNumber, Config.PageSize);
            return View(modelList);
        }
        public ActionResult Update(int id)
        {
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<DBFirstDAL.Orders, OrderModel>()
            //    .ForMember(d => d.Products, o => o.MapFrom(m=>m.ProductOrders.Select(s=>s.Products)));


            //    cfg.CreateMap<DBFirstDAL.Products, ProductOrderModel>()
            //   .ForMember(d => d.Quantity, o => o.MapFrom(m=> m.ProductOrders.FirstOrDefault(f => f.ProductId == m.Id)!=null?m.ProductOrders.FirstOrDefault(f=>f.ProductId==m.Id).Quantity:0))
            //   ;

            //});
            //config.AssertConfigurationIsValid();

            //var mapper = config.CreateMapper();

            var order = _orederRepository.Get(id);

           // var model = mapper.Map<DBFirstDAL.Orders, OrderModel>(efOrder);
            return View(order);
        }
        [HttpPost]
        public ActionResult Update(OrderModel order)
        {
            _orederRepository.UpdateType(order.Id, (int)order.TypeProgressOrder);
            return RedirectToAction("Index");
        }
        public ActionResult Delete( int id)
        {
            _orederRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}