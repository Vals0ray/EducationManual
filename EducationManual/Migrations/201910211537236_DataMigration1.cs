namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Age = c.Int(nullable: false),
                        Phone = c.String(),
                        ClassroomId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classrooms", t => t.ClassroomId)
                .Index(t => t.ClassroomId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "ClassroomId", "dbo.Classrooms");
            DropIndex("dbo.Students", new[] { "ClassroomId" });
            DropTable("dbo.Students");
        }
    }
}
