using DBFirstDAL.Repositories;
using Pyramid.Entity;
using Pyramid.Models.Cart;
using Pyramid.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class CartController : BaseController
    {
        private OrderRepository _orederRepository;
        private ProductRepository _productRepository;
        public CartController()
        {
            _productRepository = new ProductRepository();
            _orederRepository = new OrderRepository();
        }

        public ActionResult AddToCart(int productId, int quantity)
        {
            
            var line= GetCart().Lines.FirstOrDefault(i => i.Product.Id == productId);
            if (line == null)
            {
                var product = _productRepository.Get(productId);
                if (product != null)
                {

                    GetCart().AddItem(new ProductCartModel()
                    {
                        Id = product.Id,
                        Price = product.Price,
                        Title = product.Title,
                        Picture= product.ThumbnailImg
                    }
                    , quantity);
                }
            }
            else
            {
                line.Quantity += quantity;
            }
            
            return  Json(new { Action="Add", Status="ok",AllAmount=GetCart().Lines.Count()}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetQuantityToCart(int productId, int quantity)
        {
            var line = GetCart().Lines.Where(i => i.Product.Id == productId).FirstOrDefault();
            if (line!=null)
            {
                GetCart().SetQuantity(productId, quantity);
            }
            return Json(new { Action = "Add", Status = "ok", AllAmount = GetCart().Lines.Count() }, JsonRequestBehavior.AllowGet);

        }

        //public ActionResult RemoveFromCart(int productId)
        //{
        //    GetCart().RemoveLine(productId);
        //    return Json(new { Status = "ok" });
        //}

        public ActionResult PartialShortCart()
        {
            return PartialView("_PartialShortCart", GetCart());
        }
        public ActionResult ShowCart()
        {
            ViewBag.MetaTitle = "Корзина";
            return View(GetCart());
        }

        public ActionResult Checkout()
        {
            ViewBag.MetaTitle = "Подтверждение заказа";
            return View(new CheckoutModel());
        }
        [HttpPost]
        public ActionResult Checkout(CheckoutModel model)
        {
            var cart = GetCart();
            var entity = new Order()
            {
                Adress = model.Adress,
                Email = model.Email,
                Phone = model.Phone,
                TypeProgressOrder = (int)Entity.Enumerable.TypeProgressOrder.SimplePrice,
                UserName = model.Name,
                Products = cart.Lines.Select(i => new OrderProduct()
                {
                    Product = new Product() { Id = i.Product.Id },
                    Quantity=i.Quantity
                }).ToList()
            };
            _orederRepository.Add(entity);
            GetCart().Clear();
            bool flagErr = false;
            try
            {
                //_orederRepository.Add(efOrder);
                //_orederRepository.Save();
              

            }
            catch (Exception)
            {
                flagErr = true;
            }
            ViewBag.IsAddedOrder = !flagErr;

            ViewBag.MetaTitle = "Подтверждение заказа";
            return View("ResultCheckout", GetCart());
        }
        public ActionResult RemoveItem(int id)
        {
            GetCart().RemoveLine(id);
            return  Json(new { Action = "Remove", Status = "ok", AllAmount = GetCart().Lines.Count() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PartialGetCard() {
            return PartialView("PartialGetCard", GetCart());
        }

        
    }

    // GET: Cart
    
}