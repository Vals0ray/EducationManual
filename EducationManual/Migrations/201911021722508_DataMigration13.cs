namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schools", "SchoolAdminId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schools", "SchoolAdminId");
        }
    }
}
