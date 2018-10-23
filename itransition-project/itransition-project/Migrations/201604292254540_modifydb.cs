namespace itransition_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifydb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Templates", "Type_Id", "dbo.TemplateTypes");
            DropIndex("dbo.Templates", new[] { "Type_Id" });
            AddColumn("dbo.Templates", "Type", c => c.String());
            DropColumn("dbo.Templates", "Type_Id");
            DropTable("dbo.TemplateTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TemplateTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Templates", "Type_Id", c => c.Int());
            DropColumn("dbo.Templates", "Type");
            CreateIndex("dbo.Templates", "Type_Id");
            AddForeignKey("dbo.Templates", "Type_Id", "dbo.TemplateTypes", "Id");
        }
    }
}
