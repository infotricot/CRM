namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class repair : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.InvoicePayments", "Invoice_Id", "dbo.Invoices");
            //DropIndex("dbo.InvoicePayments", new[] { "Invoice_Id" });
            //DropTable("dbo.InvoicePayments");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.InvoicePayments",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Payed = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Date = c.DateTime(nullable: false),
            //            Invoice_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateIndex("dbo.InvoicePayments", "Invoice_Id");
            //AddForeignKey("dbo.InvoicePayments", "Invoice_Id", "dbo.Invoices", "Id");
        }
    }
}
