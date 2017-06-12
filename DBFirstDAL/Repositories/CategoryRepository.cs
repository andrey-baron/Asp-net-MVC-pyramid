using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBFirstDAL;
using DBFirstDAL.DataModels;

namespace DBFirstDAL.Repositories
{
    public class CategoryRepository:GenericRepository<PyramidFinalContext,Categories>
    {
        public IEnumerable<RootCategory> GetRootCategoriesWithSubs()
        {
            var rootCategories = Context.Categories.Where(i => i.ParentId == null);
            var d =new List<RootCategory>();
            foreach (var item in rootCategories)
            {
                d.Add(new RootCategory()
                {
                    Category = new CategoryWithThumbnail() {
                        Id= item.Id,
                        Thumbnail= item.CategoryImages.FirstOrDefault(f => f.CategoryId == item.Id && f.TypeImage == 1)!=null?
                        item.CategoryImages.FirstOrDefault(f=>f.CategoryId==item.Id&&f.TypeImage==1).Images:null,
                        Title=item.Title
                    } ,
                    SubCategories = Context.Categories.Where(i => i.ParentId == item.Id).Select(s=> new CategoryWithThumbnail()
                    {
                        Id = s.Id,
                        Thumbnail = s.CategoryImages.FirstOrDefault(f => f.CategoryId == s.Id && f.TypeImage == 1) != null ?
                        s.CategoryImages.FirstOrDefault(f => f.CategoryId == s.Id && f.TypeImage == 1).Images : null,
                        Title = s.Title
                    })
                });
            }
            return d;
           
        }

        public IEnumerable<CategoryWithThumbnail> GetRootCategoriesWithThumbnail(int typeThumbnail)
        {
            var rootCategories = Context.Categories.Where(i => i.ParentId == null).Select(i=>new CategoryWithThumbnail() {

            Id=i.Id,
            Thumbnail=i.CategoryImages.FirstOrDefault(s=>s.TypeImage==typeThumbnail).Images,
            Title=i.Title
            });
            
            return rootCategories;

        }

        public IEnumerable<Filters> GetFilters(int categoryId)
        {
            var efCategory = FindBy(i => i.Id == categoryId).SingleOrDefault();
            if (efCategory!=null)
            {
                return efCategory.Filters.ToList();
            }
            return new List<Filters>();
        }

       
        public void DeleteFilter(int categoryId, int filterId)
        {
            var efCategory = FindBy(i => i.Id == categoryId).SingleOrDefault();
            if (efCategory!=null)
            {
                var efFilter = Context.Filters.Find(filterId);
                efCategory.Filters.Remove(efFilter);
            }
        }

        public override void AddOrUpdate(Categories entity)
        {
            var prop = entity.GetType().GetProperty("Id");
            int id = (int)prop.GetValue(entity, null);

            var efEntity = FindBy(i=>i.Id==id).SingleOrDefault();
            if (efEntity == null)
            {
                var tmp = new List<Filters>(entity.Filters);
                entity.Filters.Clear();
                Context.Categories.Add(entity);
                Context.SaveChanges();
                foreach (var item in tmp)
                {
                    var effilter=Context.Filters.Find(item.Id);
                    if (effilter!=null)
                    {
                        entity.Filters.Add(effilter);
                    }
                }
                Context.SaveChanges();
                
                
            }
            else
            {
                if (entity.Filters!=null)
                {
                    efEntity.Filters.Clear();
                    foreach (var item in entity.Filters)
                    {
                        var efFilter = Context.Filters.Find(item.Id);
                        efEntity.Filters.Add(efFilter);
                    }
                }
                Context.Entry(efEntity).CurrentValues.SetValues(entity);
                Context.Entry(efEntity).State = System.Data.Entity.EntityState.Modified;
                
            }

        }

        public IEnumerable<Products> GetWithCheckedEnumValues(int categoryId,IEnumerable<EnumValues> enumValues)
        {
            var efCategory = FindBy(i => i.Id == categoryId).SingleOrDefault();
            if (efCategory != null)
            {
                if (enumValues.Count() > 0)
                {


                    var idValues = enumValues.Select(s => s.Id).ToList();


                    var efEnumValuetemp = Context.EnumValues.ToList();
                    var tttest = efCategory.Products.Where(i => i.EnumValues.All(p => idValues.Contains(p.Id))).ToList();
                    var te = efEnumValuetemp.Where(i => idValues.Contains(i.Id)).ToList();
                    return tttest;
                }
                
                return efCategory.Products;
            }
            return new List<Products>();
           
        }

        public void SetThumbnail(int categoryId, int imageId, int typeImage)
        {
            var efCategory = FindBy(i => i.Id == categoryId).SingleOrDefault();
            if (efCategory != null)
            {
                var efImage = efCategory.CategoryImages.FirstOrDefault(i => i.TypeImage == typeImage);
                
                if (efImage!=null)
                {
                    if (efImage.ImageId == imageId)
                    {
                        return;
                    }
                    efCategory.CategoryImages.Remove(efImage);
                }
               
                efCategory.CategoryImages.Add(new CategoryImages()
                {
                    CategoryId = categoryId,
                    ImageId = imageId,
                    TypeImage = typeImage
                });
               
            }

        }

        public void RemoveImageToCategory(int categoryId, int imageId, int typeImage)
        {
            var efCategory = FindBy(i => i.Id == categoryId).SingleOrDefault();
            if (efCategory != null)
            {
                var efImage = efCategory.CategoryImages.FirstOrDefault(i => i.ImageId == imageId && i.TypeImage == typeImage);
                if (efImage != null)
                {
                    efCategory.CategoryImages.Remove(efImage);
                }
            }

        }

        public Images GetThumbnail(int categoryId, int typeThumbnail)
        {
            return Context.Images.FirstOrDefault(i => i.CategoryImages.Any(f => f.CategoryId == categoryId && f.TypeImage == typeThumbnail));
        }

        public int GetMaxPriceFromCategory(int categoryId)
        {
            var efcat = FindBy(i => i.Id == categoryId).SingleOrDefault();
            if (efcat!=null)
            {
                if (efcat.Products!=null&& efcat.Products.Count>0)
                {
                    int max = (int)efcat.Products.Max(i => i.Price);
                    int min = (int)efcat.Products.Min(i => i.Price);
                    return max;
                }
                
            }
            return 0;
        }
        public int GetMinPriceFromCategory(int categoryId)
        {
            var efcat = FindBy(i => i.Id == categoryId).SingleOrDefault();
            if (efcat != null)
            {
                if (efcat.Products != null && efcat.Products.Count > 0)
                {
                    int min = (int)efcat.Products.Min(i => i.Price);
                return min;
                }
            }
            return 0;
        }

        public IEnumerable< Products> GetProductsByCategoryId(int id)
        {
            var efCat = FindBy(i => i.Id == id).SingleOrDefault();
            if (efCat!=null)
            {
                return efCat.Products.ToList();
            }
            return new List<Products>();
        }
    }
}
