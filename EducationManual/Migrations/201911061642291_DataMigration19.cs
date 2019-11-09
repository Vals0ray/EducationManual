namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration19 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SchoolViewModels", "SchoolAdmin_Id", "dbo.AspNetUsers");
            DropIndex("dbo.SchoolViewModels", new[] { "SchoolAdmin_Id" });
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SchoolViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SchoolAdmin_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.SchoolViewModels", "SchoolAdmin_Id");
            AddForeignKey("dbo.SchoolViewModels", "SchoolAdmin_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
