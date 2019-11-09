namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration17 : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.AspNetUsers", "SchoolId", "dbo.Schools", "SchoolId");
        }
        
        public override void Down()
        {
        }
    }
}
