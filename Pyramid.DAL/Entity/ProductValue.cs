using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.DAL.Entity
{
    public class ProductValue
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public virtual Product Product { get; set; }
    }
}
