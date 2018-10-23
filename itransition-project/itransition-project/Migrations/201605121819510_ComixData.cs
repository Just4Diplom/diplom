namespace itransition_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComixData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Balloons", "Top", c => c.String());
            AddColumn("dbo.Balloons", "Left", c => c.String());
            AddColumn("dbo.Balloons", "Width", c => c.String());
            AddColumn("dbo.Balloons", "Height", c => c.String());
            AddColumn("dbo.Frames", "BackgroundImage", c => c.String());
            AddColumn("dbo.Frames", "Top", c => c.String());
            AddColumn("dbo.Frames", "Left", c => c.String());
            AddColumn("dbo.Frames", "Width", c => c.String());
            AddColumn("dbo.Frames", "Height", c => c.String());
            DropColumn("dbo.Frames", "Position");
            DropColumn("dbo.Frames", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Frames", "Address", c => c.String());
            AddColumn("dbo.Frames", "Position", c => c.String());
            DropColumn("dbo.Frames", "Height");
            DropColumn("dbo.Frames", "Width");
            DropColumn("dbo.Frames", "Left");
            DropColumn("dbo.Frames", "Top");
            DropColumn("dbo.Frames", "BackgroundImage");
            DropColumn("dbo.Balloons", "Height");
            DropColumn("dbo.Balloons", "Width");
            DropColumn("dbo.Balloons", "Left");
            DropColumn("dbo.Balloons", "Top");
        }
    }
}
