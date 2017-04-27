namespace Pyramid.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdUniqueUserIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Users", "Login", unique: true, name: "IndexUnique");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", "IndexUnique");
        }
    }
}
