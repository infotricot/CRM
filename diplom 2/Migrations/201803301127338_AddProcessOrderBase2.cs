namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProcessOrderBase2 : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Orders", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Description", c => c.String());
        }
    }
}
