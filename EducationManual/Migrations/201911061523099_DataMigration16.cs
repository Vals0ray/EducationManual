namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration16 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Students", "ClassroomId", "dbo.Classrooms");
            DropForeignKey("dbo.AspNetUsers", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.Classrooms", "SchoolId", "dbo.Schools");
            DropPrimaryKey("dbo.Classrooms");
            DropPrimaryKey("dbo.Schools");
            DropPrimaryKey("dbo.Students");
            AddColumn("dbo.Classrooms", "ClassroomId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Schools", "SchoolId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Students", "StudentId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Classrooms", "ClassroomId");
            AddPrimaryKey("dbo.Schools", "SchoolId");
            AddPrimaryKey("dbo.Students", "StudentId");
            AddForeignKey("dbo.Students", "ClassroomId", "dbo.Classrooms", "ClassroomId");
            AddForeignKey("dbo.AspNetUsers", "SchoolId", "dbo.Schools", "SchoolId");
            AddForeignKey("dbo.Classrooms", "SchoolId", "dbo.Schools", "SchoolId");
            DropColumn("dbo.Classrooms", "Id");
            DropColumn("dbo.Schools", "Id");
            DropColumn("dbo.Students", "Id");

            RenameColumn("dbo.Classrooms", "Id", "ClassroomId");
            RenameColumn("dbo.Schools", "Id", "SchoolId");
            RenameColumn("dbo.Students", "Id", "StudentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Schools", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Classrooms", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Classrooms", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.AspNetUsers", "SchoolId", "dbo.Schools");
            DropForeignKey("dbo.Students", "ClassroomId", "dbo.Classrooms");
            DropPrimaryKey("dbo.Students");
            DropPrimaryKey("dbo.Schools");
            DropPrimaryKey("dbo.Classrooms");
            DropColumn("dbo.Students", "StudentId");
            DropColumn("dbo.Schools", "SchoolId");
            DropColumn("dbo.Classrooms", "ClassroomId");
            AddPrimaryKey("dbo.Students", "Id");
            AddPrimaryKey("dbo.Schools", "Id");
            AddPrimaryKey("dbo.Classrooms", "Id");
            AddForeignKey("dbo.Classrooms", "SchoolId", "dbo.Schools", "Id");
            AddForeignKey("dbo.AspNetUsers", "SchoolId", "dbo.Schools", "Id");
            AddForeignKey("dbo.Students", "ClassroomId", "dbo.Classrooms", "Id");
        }
    }
}
