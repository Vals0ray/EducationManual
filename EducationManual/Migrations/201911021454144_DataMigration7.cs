namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Schools", "SchoolAdminId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schools", "SchoolAdminId", c => c.Int());
        }
    }
}
