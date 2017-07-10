using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WindowsServicePyramid._1CToolModels
{
    public class ProductXMLModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public Common.TypeProductPrice TypePrice { get; set; }
        public bool Priority { get; set; }
        public List<string> CategoryTextIds { get; set; }
        public Common.TypeStatusProduct TypeStatusProduct { get; set; }

        public ProductXMLModel()
        {
            CategoryTextIds = new List<string>();
        }
    }
}