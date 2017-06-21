using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pyramid.Entity;

namespace Pyramid.Tools.Compare
{
    public class ProductCompareByTitle : IComparer<Entity.Product>
    {
        public int Compare(Product x, Product y)
        {
            if (x.Title.CompareTo(y.Title) > 0)
                return 1;
            else if (x.Title.CompareTo(y.Title) < 0)
                return -1;
            else
                return 0;
        }
    }
}