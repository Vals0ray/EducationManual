namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration14 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Schools", "SchoolAdminId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schools", "SchoolAdminId", c => c.Int());
        }
    }
}
