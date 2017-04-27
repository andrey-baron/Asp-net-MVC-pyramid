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
            //prazdnikKNAMprihOdit9415!#
            var message = string.Empty;
            if (EnterModel.VerifyPassword(model, out message))
            {
                FormsAuthentication.RedirectFromLoginPage(model.Login, false);
                TempData["Success"] = message;
                return RedirectToAction("index", "admin");

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