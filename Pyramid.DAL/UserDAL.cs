using Pyramid.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pyramid.DAL
{
    public class UserDAL
    {
        private static readonly DataContext dbContext = new DataContext();

        public static int AddOrUpdate( User user)
        {
            using (dbContext)
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

        public static User GetByLogin(string login)
        {
            using (dbContext)
            {
                return dbContext.Users.FirstOrDefault(u => u.Login == login);
            }
        }
    }
}
