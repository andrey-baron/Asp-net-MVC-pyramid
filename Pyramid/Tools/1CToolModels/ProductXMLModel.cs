using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Tools._1CToolModels
{
    public class ProductXMLModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public bool InStock { get; set; }
        public string Priority { get; set; }
        public List<string> CategoryTextIds { get; set; }

        public ProductXMLModel()
        {
            CategoryTextIds = new List<string>();
        }
    }
}