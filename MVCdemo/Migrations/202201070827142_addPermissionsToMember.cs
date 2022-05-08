namespace MVCdemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPermissionsToMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "Permissions", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "Permissions");
        }
    }
}
