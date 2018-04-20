namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFilename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "FileName", c => c.String());
            AddColumn("dbo.Invoices", "Manager_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Invoices", "Manager_Id");
            AddForeignKey("dbo.Invoices", "Manager_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "Manager_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Invoices", new[] { "Manager_Id" });
            DropColumn("dbo.Invoices", "Manager_Id");
            DropColumn("dbo.Invoices", "FileName");
        }
    }
}
