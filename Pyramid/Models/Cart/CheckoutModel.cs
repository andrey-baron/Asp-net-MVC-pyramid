using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pyramid.Models.Cart
{
    public class CheckoutModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public  string Email { get; set; }

        public string Adress { get; set; }
        public DateTime DateOrder { get; set; }
    }
}