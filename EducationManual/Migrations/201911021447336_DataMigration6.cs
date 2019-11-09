namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schools", "SchoolAdminId", c => c.Int());
            DropColumn("dbo.Schools", "SystemAdminId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schools", "SystemAdminId", c => c.Int());
            DropColumn("dbo.Schools", "SchoolAdminId");
        }
    }
}
