namespace MVCdemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpermission : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 200),
                        Value = c.String(nullable: false, maxLength: 200),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Permissions");
        }
    }
}
