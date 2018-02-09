namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKToOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Counterparty_Id", "dbo.Counterparties");
            DropForeignKey("dbo.Orders", "StatusOrder_Id", "dbo.StatusOrders");
            DropIndex("dbo.Orders", new[] { "Counterparty_Id" });
            DropIndex("dbo.Orders", new[] { "StatusOrder_Id" });
            AlterColumn("dbo.Orders", "Counterparty_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "StatusOrder_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "Counterparty_Id");
            CreateIndex("dbo.Orders", "StatusOrder_Id");
            AddForeignKey("dbo.Orders", "Counterparty_Id", "dbo.Counterparties", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "StatusOrder_Id", "dbo.StatusOrders", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "StatusOrder_Id", "dbo.StatusOrders");
            DropForeignKey("dbo.Orders", "Counterparty_Id", "dbo.Counterparties");
            DropIndex("dbo.Orders", new[] { "StatusOrder_Id" });
            DropIndex("dbo.Orders", new[] { "Counterparty_Id" });
            AlterColumn("dbo.Orders", "StatusOrder_Id", c => c.Int());
            AlterColumn("dbo.Orders", "Counterparty_Id", c => c.Int());
            CreateIndex("dbo.Orders", "StatusOrder_Id");
            CreateIndex("dbo.Orders", "Counterparty_Id");
            AddForeignKey("dbo.Orders", "StatusOrder_Id", "dbo.StatusOrders", "Id");
            AddForeignKey("dbo.Orders", "Counterparty_Id", "dbo.Counterparties", "Id");
        }
    }
}
