using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Models.Cart
{
    public class CartOrderFromEmailSendModel
    {
        public Tools.Cart Cart { get; set; }
        public CheckoutModel UserData { get; set; }
    }
}