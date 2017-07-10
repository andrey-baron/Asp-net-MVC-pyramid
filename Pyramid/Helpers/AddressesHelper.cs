using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Helpers
{
    public static class AddressesHelper
    {
        public static string GetController(this UrlHelper url)
        {
            return url.RequestContext.RouteData.Values["controller"].ToString();
        }

        public static string GetAction(this UrlHelper url)
        {
            return url.RequestContext.RouteData.Values["action"].ToString();
        }
    }
}