namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateByProcess : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Processes", "Manager_Id", "dbo.AspNetUsers");
            AddColumn("dbo.Processes", "CreatedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Processes", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Processes", "CreatedBy_Id");
            CreateIndex("dbo.Processes", "ApplicationUser_Id");
            AddForeignKey("dbo.Processes", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Processes", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Processes", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Processes", "CreatedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Processes", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Processes", new[] { "CreatedBy_Id" });
            DropColumn("dbo.Processes", "ApplicationUser_Id");
            DropColumn("dbo.Processes", "CreatedBy_Id");
            AddForeignKey("dbo.Processes", "Manager_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
