using Pyramid.Entity;
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

            DBFirstDAL.Repositories.GlobalOptionRepository globRepo = new DBFirstDAL.Repositories.GlobalOptionRepository();
           
        
            if (!globRepo.isExist("shipping"))
            {
                var newEntityObj = new GlobalOptionEntity()
                {
                    StringKey = "shipping",
                    IsHtml = true,
                    Description = "Доставка",
                    OptionContent = ""
                };
                globRepo.AddOrUpdate(newEntityObj);
            }
            

           //var _categoryRepository = new DBFirstDAL.Repositories.CategoryRepository();
           // var catAll = _categoryRepository.GetAll().ToList();

           // foreach (var item in catAll)
           // {
           //     var efCat = _categoryRepository.FindBy(i => i.Id == item.Id).SingleOrDefault();
           //     if (efCat.Seo==null)
           //     {
           //         efCat.Seo = new DBFirstDAL.Seo()
           //         {
           //             Alias = item.Title,
           //             MetaTitle = item.Title
           //         };
           //         _categoryRepository.Save();
           //     }
            //}
        }

    }
}
