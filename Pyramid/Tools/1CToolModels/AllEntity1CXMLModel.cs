using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Tools._1CToolModels
{
    public class AllEntity1CXMLModel
    {
        public IEnumerable<CategoryXMLModel> Categories { get; set; }

        public IEnumerable<ProductXMLModel> Products { get; set; }

        public AllEntity1CXMLModel()
        {
            Categories = new List<CategoryXMLModel>();
            Products = new List<ProductXMLModel>();
        }
    }
}