namespace itransition_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ratingadd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ratings", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.Comixes", "Rating_Id", "dbo.Ratings");
            DropIndex("dbo.Comixes", new[] { "Rating_Id" });
            DropIndex("dbo.Ratings", new[] { "Profile_Id" });
            AddColumn("dbo.Comixes", "RatingValue", c => c.Int(nullable: false));
            AddColumn("dbo.Ratings", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Ratings", "Comix_Id", c => c.Int());
            CreateIndex("dbo.Ratings", "User_Id");
            CreateIndex("dbo.Ratings", "Comix_Id");
            AddForeignKey("dbo.Ratings", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Ratings", "Comix_Id", "dbo.Comixes", "Id");
            DropColumn("dbo.Comixes", "Rating_Id");
            DropColumn("dbo.Ratings", "Profile_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ratings", "Profile_Id", c => c.Int());
            AddColumn("dbo.Comixes", "Rating_Id", c => c.Int());
            DropForeignKey("dbo.Ratings", "Comix_Id", "dbo.Comixes");
            DropForeignKey("dbo.Ratings", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Ratings", new[] { "Comix_Id" });
            DropIndex("dbo.Ratings", new[] { "User_Id" });
            DropColumn("dbo.Ratings", "Comix_Id");
            DropColumn("dbo.Ratings", "User_Id");
            DropColumn("dbo.Comixes", "RatingValue");
            CreateIndex("dbo.Ratings", "Profile_Id");
            CreateIndex("dbo.Comixes", "Rating_Id");
            AddForeignKey("dbo.Comixes", "Rating_Id", "dbo.Ratings", "Id");
            AddForeignKey("dbo.Ratings", "Profile_Id", "dbo.Profiles", "Id");
        }
    }
}
