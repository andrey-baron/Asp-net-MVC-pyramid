using Pyramid.Models.CategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Tools.Compare
{
    public class CompareCategoryEnumValueViewModel: IComparer<CategoryEnumValueViewModel>
    {
        public int Compare(CategoryEnumValueViewModel x, CategoryEnumValueViewModel y)
        {
            int xVal = -1;
            int yVal = -1;
            if (int.TryParse(x.Key,out xVal)&& int.TryParse(y.Key, out yVal))
            {
                if (xVal>yVal)
                    return 1;
                else if (xVal < yVal)
                    return -1;
                else
                    return 0;
            }
            return 0;
            
        }
    }
}