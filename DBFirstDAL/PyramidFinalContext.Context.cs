﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBFirstDAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PyramidFinalContext : DbContext
    {
        public PyramidFinalContext()
            : base("name=PyramidFinalContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BannerWithPoints> BannerWithPoints { get; set; }
        public virtual DbSet<CategoryImages> CategoryImages { get; set; }
        public virtual DbSet<EnumValues> EnumValues { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<Faq> Faq { get; set; }
        public virtual DbSet<FeedBackEmails> FeedBackEmails { get; set; }
        public virtual DbSet<Filters> Filters { get; set; }
        public virtual DbSet<HomeEntity> HomeEntity { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Pages> Pages { get; set; }
        public virtual DbSet<PointOnImgs> PointOnImgs { get; set; }
        public virtual DbSet<ProductImages> ProductImages { get; set; }
        public virtual DbSet<ProductOrders> ProductOrders { get; set; }
        public virtual DbSet<ProductValues> ProductValues { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswer { get; set; }
        public virtual DbSet<Seo> Seo { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<EventBanners> EventBanners { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<EventImages> EventImages { get; set; }
        public virtual DbSet<Recommendations> Recommendations { get; set; }
        public virtual DbSet<GlobalOption> GlobalOption { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<BannersOnHomePage> BannersOnHomePage { get; set; }
        public virtual DbSet<Products> Products { get; set; }
    }
}
