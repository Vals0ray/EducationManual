namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schools", "SchoolAdmin_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Schools", new[] { "SchoolAdmin_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "School_Id" });
            DropColumn("dbo.AspNetUsers", "SchoolId");
            RenameColumn(table: "dbo.AspNetUsers", name: "School_Id", newName: "SchoolId");
            DropColumn("dbo.Schools", "SchoolAdmin_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schools", "SchoolAdmin_Id", c => c.String(maxLength: 128));
            RenameColumn(table: "dbo.AspNetUsers", name: "SchoolId", newName: "School_Id");
            AddColumn("dbo.AspNetUsers", "SchoolId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "School_Id");
            CreateIndex("dbo.Schools", "SchoolAdmin_Id");
            AddForeignKey("dbo.Schools", "SchoolAdmin_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
