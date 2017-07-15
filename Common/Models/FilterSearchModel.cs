using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class FilterSearchModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<int> EventValueIds { get; set; }
    }
}
