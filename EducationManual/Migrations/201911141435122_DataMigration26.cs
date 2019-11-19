namespace EducationManual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration26 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Students", "FirstName");
            DropColumn("dbo.Students", "SecondName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "SecondName", c => c.String());
            AddColumn("dbo.Students", "FirstName", c => c.String());
        }
    }
}
