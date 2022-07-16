namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPayment : DbMigration
    {
        public override void Up()
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
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Invoices", t => t.Invoice_Id)
            //    .Index(t => t.Invoice_Id);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.InvoicePayments", "Invoice_Id", "dbo.Invoices");
            //DropIndex("dbo.InvoicePayments", new[] { "Invoice_Id" });
            //DropTable("dbo.InvoicePayments");
        }
    }
}
