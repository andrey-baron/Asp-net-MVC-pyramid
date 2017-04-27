namespace Pyramid.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCatThimbFK : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BannerWithPoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PathToImg = c.String(maxLength: 1000),
                        Points_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PointOnImgs", t => t.Points_Id)
                .Index(t => t.Points_Id);
            
            CreateTable(
                "dbo.PointOnImgs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CoordX = c.Int(nullable: false),
                        CoordY = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ThumbnailId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Available = c.Boolean(nullable: false),
                        TypePrice = c.Int(nullable: false),
                        MetaDescription = c.String(maxLength: 1000),
                        MetaTitle = c.String(maxLength: 1000),
                        MetaKeywords = c.String(maxLength: 1000),
                        IsSEOReady = c.Boolean(nullable: false),
                        Alias = c.String(maxLength: 1000),
                        DateCreation = c.DateTime(nullable: false),
                        DateChange = c.DateTime(nullable: false),
                        PointOnImg_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PointOnImgs", t => t.PointOnImg_Id)
                .Index(t => t.PointOnImg_Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Filters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.FilterValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        Product_Id = c.Int(),
                        Filter_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .ForeignKey("dbo.Filters", t => t.Filter_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Filter_Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PathInFileSystem = c.String(),
                        ImgAlt = c.String(),
                        Title = c.String(),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.ProductValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Content = c.String(),
                        ImageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.ImageId, cascadeDelete: true)
                .Index(t => t.ImageId);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.Category_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Category_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pages", "ImageId", "dbo.Images");
            DropForeignKey("dbo.BannerWithPoints", "Points_Id", "dbo.PointOnImgs");
            DropForeignKey("dbo.Products", "PointOnImg_Id", "dbo.PointOnImgs");
            DropForeignKey("dbo.ProductValues", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Images", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.ProductCategories", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Categories", "Id", "dbo.Images");
            DropForeignKey("dbo.Filters", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.FilterValues", "Filter_Id", "dbo.Filters");
            DropForeignKey("dbo.FilterValues", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductCategories", new[] { "Category_Id" });
            DropIndex("dbo.ProductCategories", new[] { "Product_Id" });
            DropIndex("dbo.Pages", new[] { "ImageId" });
            DropIndex("dbo.ProductValues", new[] { "ProductId" });
            DropIndex("dbo.Images", new[] { "Product_Id" });
            DropIndex("dbo.FilterValues", new[] { "Filter_Id" });
            DropIndex("dbo.FilterValues", new[] { "Product_Id" });
            DropIndex("dbo.Filters", new[] { "Category_Id" });
            DropIndex("dbo.Categories", new[] { "Id" });
            DropIndex("dbo.Products", new[] { "PointOnImg_Id" });
            DropIndex("dbo.BannerWithPoints", new[] { "Points_Id" });
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Pages");
            DropTable("dbo.ProductValues");
            DropTable("dbo.Images");
            DropTable("dbo.FilterValues");
            DropTable("dbo.Filters");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.PointOnImgs");
            DropTable("dbo.BannerWithPoints");
        }
    }
}
