namespace itransition_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comixDatetime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comixes", "CreationTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comixes", "CreationTime");
        }
    }
}
