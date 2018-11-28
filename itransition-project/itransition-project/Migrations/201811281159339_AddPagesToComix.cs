namespace itransition_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPagesToComix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.String(),
                        Comix_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comixes", t => t.Comix_Id)
                .Index(t => t.Comix_Id);
            
            DropColumn("dbo.Comixes", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comixes", "Image", c => c.String());
            DropForeignKey("dbo.Pages", "Comix_Id", "dbo.Comixes");
            DropIndex("dbo.Pages", new[] { "Comix_Id" });
            DropTable("dbo.Pages");
        }
    }
}
