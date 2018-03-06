namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteImage : DbMigration
    {
        public override void Up()
        {
           // DropForeignKey("dbo.Processes", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderImages", "Orderss_Id", "dbo.Orders");
            //DropIndex("dbo.Processes", new[] { "CreatedBy_Id" });
            //DropIndex("dbo.Processes", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OrderImages", new[] { "Orderss_Id" });
            //DropColumn("dbo.Processes", "Manager_Id");
            //RenameColumn(table: "dbo.Processes", name: "ApplicationUser_Id", newName: "Manager_Id");
            //AlterColumn("dbo.Processes", "ExecuteDate", c => c.DateTime(nullable: false));
            //DropColumn("dbo.Processes", "ExecuteDescription");
            //DropColumn("dbo.Processes", "CreatedBy_Id");
            DropTable("dbo.OrderImages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrderImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Link = c.String(),
                        Orderss_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            //AddColumn("dbo.Processes", "CreatedBy_Id", c => c.String(maxLength: 128));
            //AddColumn("dbo.Processes", "ExecuteDescription", c => c.String());
            //AlterColumn("dbo.Processes", "ExecuteDate", c => c.DateTime());
            //RenameColumn(table: "dbo.Processes", name: "Manager_Id", newName: "ApplicationUser_Id");
            //AddColumn("dbo.Processes", "Manager_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.OrderImages", "Orderss_Id");
            //CreateIndex("dbo.Processes", "ApplicationUser_Id");
            //CreateIndex("dbo.Processes", "CreatedBy_Id");
            AddForeignKey("dbo.OrderImages", "Orderss_Id", "dbo.Orders", "Id");
            //AddForeignKey("dbo.Processes", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
