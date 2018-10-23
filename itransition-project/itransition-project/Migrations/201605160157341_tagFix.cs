namespace itransition_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tagFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TagComixes", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.TagComixes", "Comix_Id", "dbo.Comixes");
            DropIndex("dbo.TagComixes", new[] { "Tag_Id" });
            DropIndex("dbo.TagComixes", new[] { "Comix_Id" });
            AddColumn("dbo.Tags", "Comix_Id", c => c.Int());
            CreateIndex("dbo.Tags", "Comix_Id");
            AddForeignKey("dbo.Tags", "Comix_Id", "dbo.Comixes", "Id");
            DropTable("dbo.TagComixes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TagComixes",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Comix_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Comix_Id });
            
            DropForeignKey("dbo.Tags", "Comix_Id", "dbo.Comixes");
            DropIndex("dbo.Tags", new[] { "Comix_Id" });
            DropColumn("dbo.Tags", "Comix_Id");
            CreateIndex("dbo.TagComixes", "Comix_Id");
            CreateIndex("dbo.TagComixes", "Tag_Id");
            AddForeignKey("dbo.TagComixes", "Comix_Id", "dbo.Comixes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TagComixes", "Tag_Id", "dbo.Tags", "Id", cascadeDelete: true);
        }
    }
}
