namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Classrooms", "SchoolId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "SchoolId", c => c.Int());
            CreateIndex("dbo.Classrooms", "SchoolId");
            CreateIndex("dbo.AspNetUsers", "SchoolId");
            AddForeignKey("dbo.AspNetUsers", "SchoolId", "dbo.Schools", "Id");
            AddForeignKey("dbo.Classrooms", "SchoolId", "dbo.Schools", "Id");
            DropColumn("dbo.Students", "Surname");
            DropColumn("dbo.Students", "Age");
            DropColumn("dbo.Students", "Phone");
            DropColumn("dbo.AspNetUsers", "Year");
            DropColumn("dbo.AspNetUsers", "Age");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "Phone", c => c.String());
            AddColumn("dbo.Students", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "Surname", c => c.String());
            DropForeignKey("dbo.Classrooms", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.AspNetUsers", "SchoolId", "dbo.Schools");
            DropIndex("dbo.AspNetUsers", new[] { "SchoolId" });
            DropIndex("dbo.Classrooms", new[] { "SchoolId" });
            DropColumn("dbo.AspNetUsers", "SchoolId");
            DropColumn("dbo.Classrooms", "SchoolId");
            DropTable("dbo.Schools");
        }
    }
}
