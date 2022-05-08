namespace MVCdemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSelfCollection : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Permissions", "ParentId");
            AddForeignKey("dbo.Permissions", "ParentId", "dbo.Permissions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Permissions", "ParentId", "dbo.Permissions");
            DropIndex("dbo.Permissions", new[] { "ParentId" });
        }
    }
}
