using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DBFirstDAL
{
    public class CategoryDAL
    {
        public static void AddOrUpdateEntity(Pyramid.Entity.Category entity)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {

                if (entity.Id == 0)
                {
                    dbContext.Categories.Add(new Categories() {
                    Title=entity.Title,
                    ParentId=entity.ParentId
                    });
                }
                else
                {
                    var efCategory = dbContext.Categories.Find(entity.Id);
                    dbContext.Entry(efCategory).CurrentValues.SetValues(entity);
                }
                dbContext.SaveChanges();
            }
        }

        public static void Delete(int entityId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efCat = dbContext.Categories.Find(entityId);
                if (efCat!=null)
                {
                    dbContext.Categories.Remove(efCat);
                }
                
            }
        }

        public static Pyramid.Entity.Category Get(int id)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var category = dbContext.Categories.Find(id);
                if (category != null)
                {
                    return new Pyramid.Entity.Category()
                    {
                        Title = category.Title,
                        Thumbnail = category.Images != null ? new Pyramid.Entity.Image()
                        {
                            Id =  category.Images.Id,
                            PathInFileSystem = category.Images.PathInFileSystem,
                            ServerPathImg=category.Images.ServerPathImg
                        }:null,
                        Products = (dbContext.ProductCategories.Select(i => i.Products).Select(p => new Pyramid.Entity.Product()
                        {
                            Alias = p.Alias,
                            Title = p.Title,
                            Id = p.Id

                        }).ToList()),
                        Filters = dbContext.Filters.Where(w=>w.CategoryId==category.Id).Select(i => new Pyramid.Entity.Filter()
                        {
                            Id = i.Id,
                            Title = i.Title,

                        }).ToList(),
                    };
                    
                };
                return null;
            }
        }

        public static IEnumerable<Pyramid.Entity.Category> GetAll()
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                return dbContext.Categories.Select(c => new Pyramid.Entity.Category()
                {
                    Id=c.Id,
                    Title=c.Title,
                    Thumbnail=(c.Images!=null)?new Pyramid.Entity.Image() {
                        Id =c.Images.Id,
                    ServerPathImg=c.Images.ServerPathImg}:null,
                    ThumbnailId=c.ThumbnailId
                }).ToList(); ;
            }
        }
        public static IEnumerable<Pyramid.Entity.Category> GetAllWithoutCategoryId(int catId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                return dbContext.Categories.Where(c=>c.Id!=catId).Select(c => new Pyramid.Entity.Category()
                {
                    Id = c.Id,
                    Title = c.Title,
                }).ToList(); ;
            }
        }
        public static Pyramid.Entity.Category DALToEntity( Categories category)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {


                return new Pyramid.Entity.Category()
                {
                    Id = category.Id,
                    Products = (dbContext.ProductCategories.Select(i => i.Products).Select(p => new Pyramid.Entity.Product()
                    {
                        Alias = p.Alias,
                        Title = p.Title,
                        Id = p.Id

                    }).ToList()),
                    Thumbnail = new Pyramid.Entity.Image()
                    {
                        Id = category.Images.Id,
                        PathInFileSystem = category.Images.PathInFileSystem
                    },
                    Title = category.Title,
                    //Filters = dbContext.Categories.Select(f => f.Filters).Select(i => new Pyramid.Entity.Filter() {
                    //    Id=
                    //}).ToList(),
                    };
            }
        }


    }
}
