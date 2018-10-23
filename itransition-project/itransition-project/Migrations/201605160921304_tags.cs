namespace itransition_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tags : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "Comix_Id", "dbo.Comixes");
            DropIndex("dbo.Tags", new[] { "Comix_Id" });
            CreateTable(
                "dbo.TagComixes",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Comix_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Comix_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Comixes", t => t.Comix_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Comix_Id);
            
            DropColumn("dbo.Tags", "Comix_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "Comix_Id", c => c.Int());
            DropForeignKey("dbo.TagComixes", "Comix_Id", "dbo.Comixes");
            DropForeignKey("dbo.TagComixes", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagComixes", new[] { "Comix_Id" });
            DropIndex("dbo.TagComixes", new[] { "Tag_Id" });
            DropTable("dbo.TagComixes");
            CreateIndex("dbo.Tags", "Comix_Id");
            AddForeignKey("dbo.Tags", "Comix_Id", "dbo.Comixes", "Id");
        }
    }
}
