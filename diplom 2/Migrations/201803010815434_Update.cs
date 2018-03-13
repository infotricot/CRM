namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Processes", "Manager_Id", "dbo.AspNetUsers");
            //CreateTable(
            //    "dbo.OrderImages",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Link = c.String(),
            //            Orderss_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Orders", t => t.Orderss_Id)
            //    .Index(t => t.Orderss_Id);
            
            //AddColumn("dbo.Processes", "ExecuteDescription", c => c.String());
            //AddColumn("dbo.Processes", "CreatedBy_Id", c => c.String(maxLength: 128));
            //AddColumn("dbo.Processes", "ApplicationUser_Id", c => c.String(maxLength: 128));
            //AlterColumn("dbo.Processes", "ExecuteDate", c => c.DateTime());
            //CreateIndex("dbo.Processes", "CreatedBy_Id");
            //CreateIndex("dbo.Processes", "ApplicationUser_Id");
            //AddForeignKey("dbo.Processes", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
            //AddForeignKey("dbo.Processes", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Processes", "ApplicationUser_Id", "dbo.AspNetUsers");
            //DropForeignKey("dbo.OrderImages", "Orderss_Id", "dbo.Orders");
            //DropForeignKey("dbo.Processes", "CreatedBy_Id", "dbo.AspNetUsers");
            //DropIndex("dbo.OrderImages", new[] { "Orderss_Id" });
            //DropIndex("dbo.Processes", new[] { "ApplicationUser_Id" });
            //DropIndex("dbo.Processes", new[] { "CreatedBy_Id" });
            //AlterColumn("dbo.Processes", "ExecuteDate", c => c.DateTime(nullable: false));
            //DropColumn("dbo.Processes", "ApplicationUser_Id");
            //DropColumn("dbo.Processes", "CreatedBy_Id");
            //DropColumn("dbo.Processes", "ExecuteDescription");
            //DropTable("dbo.OrderImages");
            //AddForeignKey("dbo.Processes", "Manager_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
