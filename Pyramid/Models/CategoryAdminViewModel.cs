using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Models
{
    public class CategoryAdminViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Checked { get; set; }
    }
}