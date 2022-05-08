namespace MVCdemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNNN : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "Articles", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "Articles");
        }
    }
}
