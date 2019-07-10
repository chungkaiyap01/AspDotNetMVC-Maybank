namespace Maybank.InfrastructurePersistent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201907091825 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrator",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 100),
                        Fullname = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BankAccount",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        AccountType = c.Int(nullable: false),
                        AccountNo = c.Long(nullable: false),
                        AccountBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Bank = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customer", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.BankTransaction",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BankAccountID = c.Int(nullable: false),
                        TransactionDateTime = c.DateTime(nullable: false),
                        TransactionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransferMode = c.Int(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BankAccount", t => t.BankAccountID, cascadeDelete: true)
                .Index(t => t.BankAccountID);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 100),
                        Fullname = c.String(nullable: false),
                        NRIC = c.String(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        Age = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        Photo = c.Binary(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankAccount", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.BankTransaction", "BankAccountID", "dbo.BankAccount");
            DropIndex("dbo.BankTransaction", new[] { "BankAccountID" });
            DropIndex("dbo.BankAccount", new[] { "CustomerID" });
            DropTable("dbo.Customer");
            DropTable("dbo.BankTransaction");
            DropTable("dbo.BankAccount");
            DropTable("dbo.Administrator");
        }
    }
}
