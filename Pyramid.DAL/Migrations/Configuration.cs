namespace Pyramid.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    internal sealed class Configuration : DbMigrationsConfiguration<Pyramid.DAL.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Pyramid.DAL.DataContext";
        }

        protected override void Seed(Pyramid.DAL.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            
            context.Users.AddOrUpdate(u=>u.Login,GetDefaultAdmin());
        }
        static Entity.User GetDefaultAdmin()
        {
            var pass = "123crf";
            var sha1 = new SHA512CryptoServiceProvider();
            var pswBytes = sha1.ComputeHash(Encoding.Unicode.GetBytes(pass));
            var hash = Convert.ToBase64String(pswBytes);
            return new Entity.User()
            {
                Password=hash,
                Email = "andrey0731@mail.ru",
                Login = "admin",
                UserRole = Entity.Enumerable.UserTypeRole.Admin
            };
        }
    }
}
