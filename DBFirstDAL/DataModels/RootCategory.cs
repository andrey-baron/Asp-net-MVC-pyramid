﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.DataModels
{
   public class RootCategory
    {
        public Categories Category { get; set; }
        public IEnumerable<Categories> SubCategories { get; set; }
    }
}
