namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class before : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.InvoiceItems", "Invoice_Id", "dbo.Invoices");
            //DropForeignKey("dbo.Invoices", "Order_Id", "dbo.Orders");
            //DropIndex("dbo.InvoiceItems", new[] { "Invoice_Id" });
            //DropIndex("dbo.Invoices", new[] { "Order_Id" });
            //DropTable("dbo.InvoiceItems");
            //DropTable("dbo.Invoices");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.Invoices",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Number = c.Int(nullable: false),
            //            Date = c.DateTime(nullable: false),
            //            FileName = c.String(),
            //            Payed = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Order_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.InvoiceItems",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Articul = c.String(),
            //            Product = c.String(),
            //            Amount = c.Int(nullable: false),
            //            Price = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Invoice_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateIndex("dbo.Invoices", "Order_Id");
            //CreateIndex("dbo.InvoiceItems", "Invoice_Id");
            //AddForeignKey("dbo.Invoices", "Order_Id", "dbo.Orders", "Id");
            //AddForeignKey("dbo.InvoiceItems", "Invoice_Id", "dbo.Invoices", "Id");
        }
    }
}
