using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class Product:BaseEntity
    {
       public int Id { get; set; }
       
        public string Title { get; set; }
        public Image ThumbnailImg  { get; set; }
        public double Price { get; set; }
        public bool Available { get; set; }
        public Enumerable.TypeProductPrice TypePrice { get; set; }

        public ICollection<Image> Images { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<ProductValue> ProductValues { get; set; }


        public Product()
        {
            Categories = new List<Category>();
            Images = new List<Image>();
            ProductValues = new List<ProductValue>();
        }
    }
}
