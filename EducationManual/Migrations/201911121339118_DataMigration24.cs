namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration24 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUserClaims", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.AspNetUserLogins", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.AspNetUserRoles", "Student_StudentId", "dbo.Students");
            DropIndex("dbo.AspNetUserClaims", new[] { "Student_StudentId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "Student_StudentId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "Student_StudentId" });
            RenameColumn(table: "dbo.AspNetUserClaims", name: "Student_StudentId", newName: "Student_Id");
            RenameColumn(table: "dbo.AspNetUserLogins", name: "Student_StudentId", newName: "Student_Id");
            RenameColumn(table: "dbo.AspNetUserRoles", name: "Student_StudentId", newName: "Student_Id");
            DropPrimaryKey("dbo.Students");
            AlterColumn("dbo.AspNetUserClaims", "Student_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUserLogins", "Student_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUserRoles", "Student_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Students", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Students", "Id");
            CreateIndex("dbo.AspNetUserClaims", "Student_Id");
            CreateIndex("dbo.AspNetUserLogins", "Student_Id");
            CreateIndex("dbo.AspNetUserRoles", "Student_Id");
            AddForeignKey("dbo.AspNetUserClaims", "Student_Id", "dbo.Students", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "Student_Id", "dbo.Students", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "Student_Id", "dbo.Students", "Id");
            DropColumn("dbo.Students", "StudentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "StudentId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.AspNetUserRoles", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.AspNetUserLogins", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.AspNetUserClaims", "Student_Id", "dbo.Students");
            DropIndex("dbo.AspNetUserRoles", new[] { "Student_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "Student_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "Student_Id" });
            DropPrimaryKey("dbo.Students");
            AlterColumn("dbo.Students", "Id", c => c.String());
            AlterColumn("dbo.AspNetUserRoles", "Student_Id", c => c.Int());
            AlterColumn("dbo.AspNetUserLogins", "Student_Id", c => c.Int());
            AlterColumn("dbo.AspNetUserClaims", "Student_Id", c => c.Int());
            AddPrimaryKey("dbo.Students", "StudentId");
            RenameColumn(table: "dbo.AspNetUserRoles", name: "Student_Id", newName: "Student_StudentId");
            RenameColumn(table: "dbo.AspNetUserLogins", name: "Student_Id", newName: "Student_StudentId");
            RenameColumn(table: "dbo.AspNetUserClaims", name: "Student_Id", newName: "Student_StudentId");
            CreateIndex("dbo.AspNetUserRoles", "Student_StudentId");
            CreateIndex("dbo.AspNetUserLogins", "Student_StudentId");
            CreateIndex("dbo.AspNetUserClaims", "Student_StudentId");
            AddForeignKey("dbo.AspNetUserRoles", "Student_StudentId", "dbo.Students", "StudentId");
            AddForeignKey("dbo.AspNetUserLogins", "Student_StudentId", "dbo.Students", "StudentId");
            AddForeignKey("dbo.AspNetUserClaims", "Student_StudentId", "dbo.Students", "StudentId");
        }
    }
}
