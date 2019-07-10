namespace Maybank.InfrastructurePersistent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201907091830 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customer", "Username", c => c.String());
            AlterColumn("dbo.Customer", "Password", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customer", "Password", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Customer", "Username", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
