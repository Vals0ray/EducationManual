namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUserClaims", "Student_StudentId", c => c.Int());
            AddColumn("dbo.AspNetUserLogins", "Student_StudentId", c => c.Int());
            AddColumn("dbo.AspNetUserRoles", "Student_StudentId", c => c.Int());
            AddColumn("dbo.Students", "Email", c => c.String());
            AddColumn("dbo.Students", "EmailConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Students", "PasswordHash", c => c.String());
            AddColumn("dbo.Students", "SecurityStamp", c => c.String());
            AddColumn("dbo.Students", "PhoneNumber", c => c.String());
            AddColumn("dbo.Students", "PhoneNumberConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Students", "TwoFactorEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Students", "LockoutEndDateUtc", c => c.DateTime());
            AddColumn("dbo.Students", "LockoutEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Students", "AccessFailedCount", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "Id", c => c.String());
            AddColumn("dbo.Students", "UserName", c => c.String());
            CreateIndex("dbo.AspNetUserClaims", "Student_StudentId");
            CreateIndex("dbo.AspNetUserLogins", "Student_StudentId");
            CreateIndex("dbo.AspNetUserRoles", "Student_StudentId");
            AddForeignKey("dbo.AspNetUserClaims", "Student_StudentId", "dbo.Students", "StudentId");
            AddForeignKey("dbo.AspNetUserLogins", "Student_StudentId", "dbo.Students", "StudentId");
            AddForeignKey("dbo.AspNetUserRoles", "Student_StudentId", "dbo.Students", "StudentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.AspNetUserLogins", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.AspNetUserClaims", "Student_StudentId", "dbo.Students");
            DropIndex("dbo.AspNetUserRoles", new[] { "Student_StudentId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "Student_StudentId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "Student_StudentId" });
            DropColumn("dbo.Students", "UserName");
            DropColumn("dbo.Students", "Id");
            DropColumn("dbo.Students", "AccessFailedCount");
            DropColumn("dbo.Students", "LockoutEnabled");
            DropColumn("dbo.Students", "LockoutEndDateUtc");
            DropColumn("dbo.Students", "TwoFactorEnabled");
            DropColumn("dbo.Students", "PhoneNumberConfirmed");
            DropColumn("dbo.Students", "PhoneNumber");
            DropColumn("dbo.Students", "SecurityStamp");
            DropColumn("dbo.Students", "PasswordHash");
            DropColumn("dbo.Students", "EmailConfirmed");
            DropColumn("dbo.Students", "Email");
            DropColumn("dbo.AspNetUserRoles", "Student_StudentId");
            DropColumn("dbo.AspNetUserLogins", "Student_StudentId");
            DropColumn("dbo.AspNetUserClaims", "Student_StudentId");
        }
    }
}
