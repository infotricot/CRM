namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AfterMerge1 : DbMigration
    {
        public override void Up()
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
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Orders", t => t.Order_Id)
            //    .Index(t => t.Order_Id);
            
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
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Invoices", t => t.Invoice_Id)
            //    .Index(t => t.Invoice_Id);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Invoices", "Order_Id", "dbo.Orders");
            //DropForeignKey("dbo.InvoiceItems", "Invoice_Id", "dbo.Invoices");
            //DropIndex("dbo.InvoiceItems", new[] { "Invoice_Id" });
            //DropIndex("dbo.Invoices", new[] { "Order_Id" });
            //DropTable("dbo.InvoiceItems");
            //DropTable("dbo.Invoices");
        }
    }
}
