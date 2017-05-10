//using Pyramid.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DBFirstDAL
{
    public class UserDAL
    {
        //private static readonly DataContext dbContext = new DataContext();

        public static int AddOrUpdate( Users user)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                if (user.Id == 0)
                {
                    dbContext.Users.Add(user);
                }
                else
                {
                    var efuser = dbContext.Users.Find(user.Id);
                    dbContext.Entry(efuser).CurrentValues.SetValues(user);
                }
                dbContext.SaveChanges();
                return user.Id;

            }
        }

        public static Users GetByLogin(string login)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                return dbContext.Users.FirstOrDefault(u => u.Login == login);
            }
        }
        public static Pyramid.Entity.User DALToEntity(Users user)
        {
            return new Pyramid.Entity.User()
            {
                Email = user.Email,
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                UserRole = (Pyramid.Entity.Enumerable.UserTypeRole)user.UserRole
            };
        }
        public static  Users EntityToDAL(Pyramid.Entity.User user)
        {
            return new Users()
            {
                Email = user.Email,
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                UserRole =(int) user.UserRole
            };
        }
    }
}
