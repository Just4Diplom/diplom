namespace itransition_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateComixModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Balloons", "Frame_Id", "dbo.Frames");
            DropForeignKey("dbo.Frames", "Page_Id", "dbo.Pages");
            DropForeignKey("dbo.Pages", "Template_Id", "dbo.Templates");
            DropForeignKey("dbo.Pages", "Comix_Id", "dbo.Comixes");
            DropIndex("dbo.Balloons", new[] { "Frame_Id" });
            DropIndex("dbo.Pages", new[] { "Template_Id" });
            DropIndex("dbo.Pages", new[] { "Comix_Id" });
            DropIndex("dbo.Frames", new[] { "Page_Id" });
            AddColumn("dbo.Comixes", "Image", c => c.Binary());
            DropColumn("dbo.Balloons", "Frame_Id");
            DropTable("dbo.Pages");
            DropTable("dbo.Frames");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Frames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BackgroundImage = c.String(),
                        Top = c.String(),
                        Left = c.String(),
                        Width = c.String(),
                        Height = c.String(),
                        Page_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Template_Id = c.Int(),
                        Comix_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Balloons", "Frame_Id", c => c.Int());
            DropColumn("dbo.Comixes", "Image");
            CreateIndex("dbo.Frames", "Page_Id");
            CreateIndex("dbo.Pages", "Comix_Id");
            CreateIndex("dbo.Pages", "Template_Id");
            CreateIndex("dbo.Balloons", "Frame_Id");
            AddForeignKey("dbo.Pages", "Comix_Id", "dbo.Comixes", "Id");
            AddForeignKey("dbo.Pages", "Template_Id", "dbo.Templates", "Id");
            AddForeignKey("dbo.Frames", "Page_Id", "dbo.Pages", "Id");
            AddForeignKey("dbo.Balloons", "Frame_Id", "dbo.Frames", "Id");
        }
    }
}
