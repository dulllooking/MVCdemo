namespace MVCdemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMembers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(nullable: false, maxLength: 200),
                        Password = c.String(nullable: false, maxLength: 200),
                        PasswordSalt = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 200),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Members");
        }
    }
}
