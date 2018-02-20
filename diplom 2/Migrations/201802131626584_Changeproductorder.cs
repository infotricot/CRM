namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changeproductorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Comments", c => c.String());
            DropColumn("dbo.ProductInOrders", "Price");
            DropColumn("dbo.ProductInOrders", "Comments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductInOrders", "Comments", c => c.String());
            AddColumn("dbo.ProductInOrders", "Price", c => c.String());
            DropColumn("dbo.Orders", "Comments");
        }
    }
}
