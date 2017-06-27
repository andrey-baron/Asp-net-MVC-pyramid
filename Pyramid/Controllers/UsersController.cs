using Pyramid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Pyramid.Controllers
{
    public class UsersController:Controller
    {


        /// <summary>
        /// Страница авторизации.
        /// </summary>
        /// <returns></returns>
        public ActionResult SignIn()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SignIn(EnterModel model)
        {
            
            var message = string.Empty;
            if (EnterModel.VerifyPassword(model, out message))
            {
                FormsAuthentication.RedirectFromLoginPage(model.Login, false);
                TempData["Success"] = message;
                //var u = FormsAuthentication.GetRedirectUrl(model.Login, false);
                
                return RedirectToAction("Index","admin");

            }
            TempData["Error"] = message;
            return RedirectToAction("signin");
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("signin");
        }

        

    }
}