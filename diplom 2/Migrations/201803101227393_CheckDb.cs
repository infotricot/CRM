namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckDb : DbMigration
    {
        public override void Up()
        {
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
            
            //AddColumn("dbo.Processes", "IsExecuted", c => c.Boolean());
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.OrderImages", "Orderss_Id", "dbo.Orders");
            //DropIndex("dbo.OrderImages", new[] { "Orderss_Id" });
            //DropColumn("dbo.Processes", "IsExecuted");
            //DropTable("dbo.OrderImages");
        }
    }
}
