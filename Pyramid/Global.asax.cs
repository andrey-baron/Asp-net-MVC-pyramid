using Entity;
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
            if (!globRepo.isExist("footnote"))
            {
                var newEntityObj = new GlobalOptionEntity()
                {
                    StringKey = "footnote",
                    IsHtml = true,
                    Description = "Текст сноски когда товар доступен к заказу",
                    OptionContent = ""
                };
                globRepo.AddOrUpdate(newEntityObj);
            }



            //var _categoryRepository = new DBFirstDAL.Repositories.CategoryRepository();
            //var catAll = _categoryRepository.GetAll().ToList();

            //foreach (var item in catAll)
            //{
            //    _categoryRepository.InitSeo(item.Id);
            //}

            //var repoProducts = new DBFirstDAL.Repositories.ProductRepository();
            //var products = repoProducts.GetAll();
            //foreach (var item in products)
            //{
            //    repoProducts.InitSeo(item.Id);
            //}

            var repoRoute = new DBFirstDAL.Repositories.RouteItemRepository();
            //foreach (var item in catAll)
            //{
            //    repoRoute.AddOrUpdate(new RouteItem() {
            //        ActionName="Index",
            //        ControllerName="Category",
            //        ContentId=item.Id,
            //        Type= Common.TypeEntityFromRouteEnum.CategoryType,

            //    });
            //}
            //foreach (var item in products)
            //{
            //    repoRoute.AddOrUpdate(new RouteItem()
            //    {
            //        ActionName = "Index",
            //        ControllerName = "Product",
            //        ContentId = item.Id,
            //        Type = Common.TypeEntityFromRouteEnum.ProductType,

            //    });
            //}

            var pageRepo = new DBFirstDAL.Repositories.PageRepository();
            var allPages = pageRepo.GetAll();
            //foreach (var item in allPages)
            //{
            //    pageRepo.InitSeo(item.Id);
            //}
            foreach (var item in allPages)
            {
                repoRoute.AddOrUpdate(new RouteItem()
                {
                    ActionName = "Index",
                    ControllerName = "Page",
                    ContentId = item.Id,
                    Type = Common.TypeEntityFromRouteEnum.PageType,

                });
            }
        }

    }
}
