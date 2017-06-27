using Pyramid.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Tools.Compare
{
    public class ProductCompareByPopular : IComparer<Entity.Product>
    {
        public int Compare(Product x, Product y)
        {
            if (x.PopularCount > y.PopularCount)
                return 1;
            else if (x.PopularCount < y.PopularCount)
                return -1;
            else
                return 0;
        }
    }
}