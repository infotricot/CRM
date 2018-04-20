namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invoice2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Articul = c.String(),
                        Product = c.String(),
                        Amount = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Invoice_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.Invoice_Id)
                .Index(t => t.Invoice_Id);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Date = c.Int(nullable: false),
                        Counterparty_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Counterparties", t => t.Counterparty_Id)
                .Index(t => t.Counterparty_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceItems", "Invoice_Id", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "Counterparty_Id", "dbo.Counterparties");
            DropIndex("dbo.Invoices", new[] { "Counterparty_Id" });
            DropIndex("dbo.InvoiceItems", new[] { "Invoice_Id" });
            DropTable("dbo.Invoices");
            DropTable("dbo.InvoiceItems");
        }
    }
}
