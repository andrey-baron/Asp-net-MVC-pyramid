namespace Pyramid.DAL
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Linq;

    public class DataContext : DbContext
    {
        // Контекст настроен для использования строки подключения "DataContext" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "Pyramid.DAL.DataContext" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "DataContext" 
        // в файле конфигурации приложения.
        public DataContext()
            : base("name=DataContext")
        {
        }

        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Entity.Product> Products { get; set; }
        public virtual DbSet<Entity.Category> Categories { get; set; }
        public virtual DbSet<Entity.Filter> Filters { get; set; }
        public virtual DbSet<Entity.FilterValue> FilterValues { get; set; }
        public virtual DbSet<Entity.BannerWithPoints> BannerWithPoints { get; set; }
        public virtual DbSet<Entity.Image> Images { get; set; }
        public virtual DbSet<Entity.Page> Pages { get; set; }
        public virtual DbSet<Entity.User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // использование Fluent API
            modelBuilder.Entity<Entity.Product>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products);
            
            modelBuilder.Entity<Entity.Product>()
                .HasMany(c => c.ProductValues)
                .WithRequired(c => c.Product)
                .HasForeignKey(c=>c.ProductId);

           modelBuilder.Entity<Entity.Product>()
                .HasOptional(p => p.Images)
                .WithOptionalDependent();

            modelBuilder.Entity<Entity.Product>()
                .HasOptional(p => p.ThumbnailImg)
                .WithOptionalDependent();

            modelBuilder.Entity<Entity.Category>().HasKey(k => k.Id);

            modelBuilder.Entity<Entity.Category>()
                .Property(p=>p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity) ;
                

            modelBuilder.Entity<Entity.Category>()
                .HasMany(c => c.Filters)
                .WithRequired(c => c.Category);

            modelBuilder.Entity<Entity.Category>()
               .HasOptional(p => p.Thumbnail)
                .WithOptionalDependent();
            ;

            modelBuilder.Entity<Entity.Filter>()
               .HasMany(c => c.FilterValues)
               .WithRequired(c => c.Filter);

            modelBuilder.Entity<Entity.User>()
                .Property(u => u.Login)
                .HasMaxLength(100)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new[] 
                    {
                        new IndexAttribute("IndexUnique") { IsUnique = true }
                    }));
            base.OnModelCreating(modelBuilder);
        }
    }
    
}