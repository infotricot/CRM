namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderInInvoice : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invoices", "Counterparty_Id", "dbo.Counterparties");
            DropForeignKey("dbo.Invoices", "Manager_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Invoices", new[] { "Counterparty_Id" });
            DropIndex("dbo.Invoices", new[] { "Manager_Id" });
            AddColumn("dbo.Invoices", "Order_Id", c => c.Int());
            CreateIndex("dbo.Invoices", "Order_Id");
            AddForeignKey("dbo.Invoices", "Order_Id", "dbo.Orders", "Id");
            DropColumn("dbo.Invoices", "Counterparty_Id");
            DropColumn("dbo.Invoices", "Manager_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Invoices", "Manager_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Invoices", "Counterparty_Id", c => c.Int());
            DropForeignKey("dbo.Invoices", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Invoices", new[] { "Order_Id" });
            DropColumn("dbo.Invoices", "Order_Id");
            CreateIndex("dbo.Invoices", "Manager_Id");
            CreateIndex("dbo.Invoices", "Counterparty_Id");
            AddForeignKey("dbo.Invoices", "Manager_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Invoices", "Counterparty_Id", "dbo.Counterparties", "Id");
        }
    }
}
