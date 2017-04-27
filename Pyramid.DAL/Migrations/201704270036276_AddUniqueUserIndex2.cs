namespace Pyramid.DAL.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueUserIndex2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Login", c => c.String(maxLength: 100,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "IndexUnique",
                        new AnnotationValues(oldValue: "IndexAnnotation: { Name: IndexUnique, IsUnique: True }", newValue: null)
                    },
                }));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Login", c => c.String(
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "IndexUnique",
                        new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { Name: IndexUnique, IsUnique: True }")
                    },
                }));
        }
    }
}
