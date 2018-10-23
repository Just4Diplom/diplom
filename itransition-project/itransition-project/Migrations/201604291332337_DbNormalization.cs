namespace itransition_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbNormalization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BalloonTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TemplateTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Balloons", "Type_Id", c => c.Int());
            AddColumn("dbo.Templates", "Type_Id", c => c.Int());
            AddColumn("dbo.Ratings", "Condition", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Balloons", "Type_Id");
            CreateIndex("dbo.Templates", "Type_Id");
            AddForeignKey("dbo.Balloons", "Type_Id", "dbo.BalloonTypes", "Id");
            AddForeignKey("dbo.Templates", "Type_Id", "dbo.TemplateTypes", "Id");
            DropColumn("dbo.Frames", "Type");
            DropColumn("dbo.Balloons", "Type");
            DropColumn("dbo.Templates", "Type");
            DropColumn("dbo.Ratings", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ratings", "Type", c => c.String());
            AddColumn("dbo.Templates", "Type", c => c.String());
            AddColumn("dbo.Balloons", "Type", c => c.String());
            AddColumn("dbo.Frames", "Type", c => c.String());
            DropForeignKey("dbo.Templates", "Type_Id", "dbo.TemplateTypes");
            DropForeignKey("dbo.Balloons", "Type_Id", "dbo.BalloonTypes");
            DropIndex("dbo.Templates", new[] { "Type_Id" });
            DropIndex("dbo.Balloons", new[] { "Type_Id" });
            DropColumn("dbo.Ratings", "Condition");
            DropColumn("dbo.Templates", "Type_Id");
            DropColumn("dbo.Balloons", "Type_Id");
            DropTable("dbo.TemplateTypes");
            DropTable("dbo.BalloonTypes");
        }
    }
}
