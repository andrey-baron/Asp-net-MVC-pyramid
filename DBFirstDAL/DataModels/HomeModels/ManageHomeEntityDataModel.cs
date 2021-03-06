﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.DataModels.HomeModels
{
    public class ManageHomeEntityDataModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Categories Categories { get; set; }
        public Products Products { get; set; }
        public BannerWithPoints BannerWithPoints { get; set; }
        public Faq Faq { get; set; }
        public ManageHomeEntityDataModel()
        {
        }
    }
}
