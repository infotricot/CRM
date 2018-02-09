namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNameToProductInOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductInOrders", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductInOrders", "Name");
        }
    }
}
