using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.DAL.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public Enumerable.UserTypeRole UserRole { get; set; }
        public string Password { get; set; }
    }
}
