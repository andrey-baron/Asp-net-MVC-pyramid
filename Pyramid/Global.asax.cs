using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Pyramid
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var admin=DBFirstDAL.UserDAL.GetByLogin("admin");
            if (admin==null)
            {
                DBFirstDAL.UserDAL.AddOrUpdate(
                    DBFirstDAL.UserDAL.EntityToDAL(
                        Pyramid.Tools.Cryptography.GetDefaultAdmin()
                        ));
            }
        }

    }
}
