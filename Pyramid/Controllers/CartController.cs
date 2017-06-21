using DBFirstDAL.Repositories;
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

            var line= GetCart().Lines.Where(i => i.Product.Id == productId).FirstOrDefault();
            if (line == null)
            {
                var efProduct = _productRepository.FindBy(g => g.Id == productId).SingleOrDefault();
                if (efProduct != null)
                {

                    GetCart().AddItem(new ProductCartModel()
                    {
                        Id = efProduct.Id,
                        Price = efProduct.Price,
                        Title = efProduct.Title
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
            return View(GetCart());
        }

        public ActionResult Checkout()
        {
            
            return View(new CheckoutModel());
        }
        [HttpPost]
        public ActionResult Checkout(CheckoutModel model)
        {
            var cart = GetCart();
            var efOrder = new DBFirstDAL.Orders()
            {
                Adress = model.Adress,
                Email = model.Email,
                Phone = model.Phone,
                TypeProgressOrder = 1,
                UserName = model.Name,
                ProductOrders = cart.Lines.Select(i => new DBFirstDAL.ProductOrders()
                {
                    ProductId = i.Product.Id,
                    Quantity=i.Quantity
                }).ToList()
            };
            _orederRepository.AddOrUpdate(efOrder);
            bool flagErr = false;
            try
            {
                _orederRepository.Add(efOrder);

                GetCart().Clear();

            }
            catch (Exception)
            {
                flagErr = true;
            }
            ViewBag.IsAddedOrder = !flagErr;

            return View("ResultCheckout",cart);
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