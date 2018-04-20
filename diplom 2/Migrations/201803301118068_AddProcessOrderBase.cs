namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProcessOrderBase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "IsExecuted", c => c.Boolean());
            DropColumn("dbo.Orders", "CreatedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "CreatedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Orders", "IsExecuted");
            DropColumn("dbo.Orders", "CreateDate");
        }
    }
}
