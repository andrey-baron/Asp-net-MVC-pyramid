using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.Repositories
{
    public class ProductRepository: GenericRepository<PyramidFinalContext, Products>
    {
        public IEnumerable<EnumValues> GetAllEnumValues(int productId)
        {
            var filter = FindBy(i => i.Id == productId).SingleOrDefault();
            if (filter != null)
            {
                return filter.EnumValues;
            }
            return new List<EnumValues>();
        }

        public void DeleteEnumValue(int id, int enumValueId)
        {
            var filter = FindBy(i => i.Id == id).SingleOrDefault();
            if (filter != null)
            {
                var enumvalue = Context.EnumValues.Find(enumValueId);
                filter.EnumValues.Remove(enumvalue);

            }
        }

        public override void AddOrUpdate(Products entity)
        {
            var prop = entity.GetType().GetProperty("Id");
            int id = (int)prop.GetValue(entity, null);

            var efEntity = Context.Products.Find(id);
            if (efEntity == null)
            {
                Context.Products.Add(entity);
                Context.Entry(entity).State = System.Data.Entity.EntityState.Added;
                Context.SaveChanges();
            }
            else
            {
                efEntity.Categories.Clear();
                foreach (var item in entity.Categories)
                {
                    var efCategory = Context.Categories.Find(item.Id);
                    efEntity.Categories.Add(efCategory);
                }
                efEntity.EnumValues.Clear();
                foreach (var item in entity.EnumValues)
                {
                    var efEnumValues = Context.EnumValues.Find(item.Id);
                    efEntity.EnumValues.Add(efEnumValues);
                }
                Context.Entry(efEntity).CurrentValues.SetValues(entity);
                Context.Entry(efEntity).State = System.Data.Entity.EntityState.Modified;

            }
            if (entity.ProductImages!=null)
            {
                var thumbnail = entity.ProductImages.FirstOrDefault(i => i.TypeImage == (int)Pyramid.Entity.Enumerable.TypeImage.Thumbnail);
                if (thumbnail != null)
                {
                   var efThumbnail= Context.ProductImages.FirstOrDefault(i => i.ImageId == thumbnail.ImageId);
                    if (efThumbnail!=null)
                    {
                        Context.ProductImages.Remove(efThumbnail);
                    }
                   
                    Context.ProductImages.Add(new ProductImages() {
                        ImageId =thumbnail.ImageId,
                        ProductId=thumbnail.ProductId,
                        TypeImage=thumbnail.TypeImage
                    });
                }
                
            }

        }

        public Images GetThumbnail(int productId, int typeThumbnail )
        {
            return Context.Images.FirstOrDefault(i => i.ProductImages.All(f => f.ProductId == productId && f.TypeImage == typeThumbnail));
        }

        public IEnumerable<Images> GetGalleryImage(int productId, int typeGallery)
        {
            return Context.Images.Where(i => i.ProductImages.All(f => f.ProductId == productId && f.TypeImage == typeGallery));
        }
    }
}
