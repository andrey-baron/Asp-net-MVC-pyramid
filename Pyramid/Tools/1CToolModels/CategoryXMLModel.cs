using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Tools._1CToolModels
{
    public class CategoryXMLModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ParentId { get; set; }

        //public IEnumerable<CategoryXMLModel> InnerCategories { get; set; }

        //public CategoryXMLModel()
        //{
        //    InnerCategories = new List<CategoryXMLModel>();
        //}
    }
}