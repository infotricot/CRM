namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTypeDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Invoices", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Invoices", "Date", c => c.Int(nullable: false));
        }
    }
}
