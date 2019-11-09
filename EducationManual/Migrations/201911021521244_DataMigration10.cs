namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration10 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "SchoolId", "dbo.Schools");
            AddColumn("dbo.Schools", "SchoolAdmin_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "School_Id", c => c.Int());
            CreateIndex("dbo.Schools", "SchoolAdmin_Id");
            CreateIndex("dbo.AspNetUsers", "School_Id");
            AddForeignKey("dbo.Schools", "SchoolAdmin_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "School_Id", "dbo.Schools", "Id");
            DropColumn("dbo.Schools", "SchoolAdminId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schools", "SchoolAdminId", c => c.String());
            DropForeignKey("dbo.AspNetUsers", "School_Id", "dbo.Schools");
            DropForeignKey("dbo.Schools", "SchoolAdmin_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "School_Id" });
            DropIndex("dbo.Schools", new[] { "SchoolAdmin_Id" });
            DropColumn("dbo.AspNetUsers", "School_Id");
            DropColumn("dbo.Schools", "SchoolAdmin_Id");
            AddForeignKey("dbo.AspNetUsers", "SchoolId", "dbo.Schools", "Id");
        }
    }
}
