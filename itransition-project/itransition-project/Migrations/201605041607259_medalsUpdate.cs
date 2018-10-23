namespace itransition_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class medalsUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Medals", "Name", c => c.String());
            AddColumn("dbo.Medals", "Image", c => c.String());
            DropColumn("dbo.Medals", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Medals", "Type", c => c.String());
            DropColumn("dbo.Medals", "Image");
            DropColumn("dbo.Medals", "Name");
        }
    }
}
