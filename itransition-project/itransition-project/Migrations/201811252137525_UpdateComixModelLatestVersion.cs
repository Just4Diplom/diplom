namespace itransition_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateComixModelLatestVersion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comixes", "Image", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comixes", "Image", c => c.Binary());
        }
    }
}
