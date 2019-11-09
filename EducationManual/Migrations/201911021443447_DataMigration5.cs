namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schools", "SystemAdminId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schools", "SystemAdminId");
        }
    }
}
