using Pyramid.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Tools.Compare
{
    public class ProductCompareByPrice : IComparer<Entity.Product>
    {
        public int Compare(Product x, Product y)
        {
            if (x.Price > y.Price)
                return 1;
            else if (x.Price < y.Price)
                return -1;
            else
                return 0;
        }
    }
}