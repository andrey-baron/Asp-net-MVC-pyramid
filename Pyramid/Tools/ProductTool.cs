using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Tools
{
    public class ProductTool
    {
        public static string GetClassByTypePrice(int typePrice)
        {
            string response = "";
            switch (typePrice)
            {
                case (int)Common.TypeProductPrice.SimplePrice:
                    
                    break;
                case (int)Common.TypeProductPrice.NewPrice:
                    response = "product__item-price_new";
                    break;
                
                case (int)Common.TypeProductPrice.SpecialOffer:
                    response = "product__item-price_limited"; 
                    break;
                default:
                    break;
            }
            return response;
        }
    }
}