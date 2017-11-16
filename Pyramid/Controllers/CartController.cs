using DBFirstDAL.Repositories;
using Pyramid.Entity;
using Pyramid.Models.Cart;
using Pyramid.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Pyramid.Tools.Mailer;

namespace Pyramid.Controllers
{
    public class CartController : BaseController
    {
        private OrderRepository _orederRepository;
        private ProductRepository _productRepository;
        private FeedBackEmailRepository _feedBackEmailRepository;
        public CartController()
        {
            _productRepository = new ProductRepository();
            _orederRepository = new OrderRepository();
            _feedBackEmailRepository = new FeedBackEmailRepository();
        }

        public ActionResult AddToCart(int productId, int quantity)
        {

            var line = GetCart().Lines.FirstOrDefault(i => i.Product.Id == productId);
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
                        Picture = product.ThumbnailImg
                    }
                    , quantity);
                }
            }
            else
            {
                line.Quantity += quantity;
            }

            return Json(new { Action = "Add", Status = "ok", AllAmount = GetCart().Lines.Count() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetQuantityToCart(int productId, int quantity)
        {
            var line = GetCart().Lines.Where(i => i.Product.Id == productId).FirstOrDefault();
            if (line != null)
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
            ValidateModel(model);
            var cart = new Cart( GetCart());
            
            var emailModel = new CartOrderFromEmailSendModel()
            {
                Cart=cart,
                UserData=model
            };
            var message = RenderViewToString(this.ControllerContext, "/Views/Cart/_PartialNewOrderFromEmailSend.cshtml", emailModel, true);

           
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
                    Quantity = i.Quantity
                }).ToList(),
                DateOrder=DateTime.Now
            };
            
            MailerMessage mailerMessage = new MailerMessage();
            mailerMessage.Body = message;
            mailerMessage.Subject = "Заказ с сайта Пирамида строй";
            mailerMessage.To = _feedBackEmailRepository.GetAll().Select(i => i.Email).ToList();
            mailerMessage.SenderName = "Pyramid";

            if (mailerMessage.To.Count > 0)
            {
                Mailer.Send(mailerMessage);
            }
            
            GetCart().Clear();
            bool flagErr = false;
            try
            {
                _orederRepository.Add(entity);
            }
            catch (Exception)
            {
                flagErr = true;
            }
            ViewBag.IsAddedOrder = !flagErr;

            ViewBag.MetaTitle = "Подтверждение заказа";
            return View("ResultCheckout", cart);
        }
        public ActionResult RemoveItem(int id)
        {
            GetCart().RemoveLine(id);
            return Json(new { Action = "Remove", Status = "ok", AllAmount = GetCart().Lines.Count() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PartialGetCard()
        {
            return PartialView("PartialGetCard", GetCart());
        }

        public ActionResult CheckoutOneClick(int id)
        {
            var product = _productRepository.Get(id);
            ViewBag.Product = product;
            ViewBag.MetaTitle = "Подтверждение заказа";
            return View(new CheckoutModel());
        }
        [HttpPost]
        public ActionResult CheckoutOneClick(CheckoutModel model, int productId)
        {
            ValidateModel(model);
            var product=_productRepository.Get(productId);

            List<CartLine> lineCollection = new List<CartLine>();
            lineCollection.Add(new CartLine()
            {
                Quantity = 1,
                Product = new ProductCartModel()
                {
                    Id = product.Id,
                    Price = product.Price,
                    Title = product.Title
                }
            });
            var emailModel = new CartOrderFromEmailSendModel()
            {
                Cart = new Cart(lineCollection),
                UserData = model
            };
            var message = RenderViewToString(this.ControllerContext, "/Views/Cart/_PartialNewOrderFromEmailSend.cshtml", emailModel, true);

            var entity = new Order()
            {
                Adress = model.Adress,
                Email = model.Email,
                Phone = model.Phone,
                TypeProgressOrder = (int)Entity.Enumerable.TypeProgressOrder.SimplePrice,
                UserName = model.Name,
                Products = new List<OrderProduct>(new OrderProduct[] { new OrderProduct() {
                    Product=new Product() {Id=productId },
                    Quantity=1
                } }),
            };
            MailerMessage mailerMessage = new MailerMessage();
            mailerMessage.Body = message;
            mailerMessage.Subject = "Заказ с сайта Пирамида строй";
            mailerMessage.To = _feedBackEmailRepository.GetAll().Select(i => i.Email).ToList();
            mailerMessage.SenderName = "Pyramid";

            if (mailerMessage.To.Count > 0)
            {
                Mailer.Send(mailerMessage);
            }

            bool flagErr = false;
            try
            {
                _orederRepository.Add(entity);
            }
            catch (Exception ex)
            {
                flagErr = true;
            }
            ViewBag.IsAddedOrder = !flagErr;
            return View("ResultCheckoutOneClick");
        }
    }
}