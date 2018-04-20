namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPayed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "Payed", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "Payed");
        }
    }
}
