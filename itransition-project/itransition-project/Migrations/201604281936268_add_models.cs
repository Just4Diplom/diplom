namespace itransition_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_models : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Photo = c.String(),
                        About = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Comixes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AgeRating_Id = c.Int(),
                        Author_Id = c.String(maxLength: 128),
                        Rating_Id = c.Int(),
                        Profile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AgeRatings", t => t.AgeRating_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.Ratings", t => t.Rating_Id)
                .ForeignKey("dbo.Profiles", t => t.Profile_Id)
                .Index(t => t.AgeRating_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Rating_Id)
                .Index(t => t.Profile_Id);
            
            CreateTable(
                "dbo.AgeRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Templates", t => t.Template_Id)
                .ForeignKey("dbo.Comixes", t => t.Comix_Id)
                .Index(t => t.Template_Id)
                .Index(t => t.Comix_Id);
            
            CreateTable(
                "dbo.Frames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Position = c.String(),
                        Address = c.String(),
                        Page_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pages", t => t.Page_Id)
                .Index(t => t.Page_Id);
            
            CreateTable(
                "dbo.Balloons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Text = c.String(),
                        Frame_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Frames", t => t.Frame_Id)
                .Index(t => t.Frame_Id);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PositionsCount = c.Int(nullable: false),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Profile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profiles", t => t.Profile_Id)
                .Index(t => t.Profile_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Time = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                        Profile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.Profiles", t => t.Profile_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Profile_Id);
            
            CreateTable(
                "dbo.Medals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Profile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profiles", t => t.Profile_Id)
                .Index(t => t.Profile_Id);
            
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
            
            AddColumn("dbo.AspNetUsers", "Language", c => c.String());
            AddColumn("dbo.AspNetUsers", "Theme", c => c.String());
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Profiles", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.Medals", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.Comments", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.Comments", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comixes", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.TagComixes", "Comix_Id", "dbo.Comixes");
            DropForeignKey("dbo.TagComixes", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Comixes", "Rating_Id", "dbo.Ratings");
            DropForeignKey("dbo.Pages", "Comix_Id", "dbo.Comixes");
            DropForeignKey("dbo.Pages", "Template_Id", "dbo.Templates");
            DropForeignKey("dbo.Frames", "Page_Id", "dbo.Pages");
            DropForeignKey("dbo.Balloons", "Frame_Id", "dbo.Frames");
            DropForeignKey("dbo.Comixes", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comixes", "AgeRating_Id", "dbo.AgeRatings");
            DropIndex("dbo.TagComixes", new[] { "Comix_Id" });
            DropIndex("dbo.TagComixes", new[] { "Tag_Id" });
            DropIndex("dbo.Medals", new[] { "Profile_Id" });
            DropIndex("dbo.Comments", new[] { "Profile_Id" });
            DropIndex("dbo.Comments", new[] { "Author_Id" });
            DropIndex("dbo.Ratings", new[] { "Profile_Id" });
            DropIndex("dbo.Balloons", new[] { "Frame_Id" });
            DropIndex("dbo.Frames", new[] { "Page_Id" });
            DropIndex("dbo.Pages", new[] { "Comix_Id" });
            DropIndex("dbo.Pages", new[] { "Template_Id" });
            DropIndex("dbo.Comixes", new[] { "Profile_Id" });
            DropIndex("dbo.Comixes", new[] { "Rating_Id" });
            DropIndex("dbo.Comixes", new[] { "Author_Id" });
            DropIndex("dbo.Comixes", new[] { "AgeRating_Id" });
            DropIndex("dbo.Profiles", new[] { "User_Id" });
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "Theme");
            DropColumn("dbo.AspNetUsers", "Language");
            DropTable("dbo.TagComixes");
            DropTable("dbo.Medals");
            DropTable("dbo.Comments");
            DropTable("dbo.Tags");
            DropTable("dbo.Ratings");
            DropTable("dbo.Templates");
            DropTable("dbo.Balloons");
            DropTable("dbo.Frames");
            DropTable("dbo.Pages");
            DropTable("dbo.AgeRatings");
            DropTable("dbo.Comixes");
            DropTable("dbo.Profiles");
        }
    }
}
