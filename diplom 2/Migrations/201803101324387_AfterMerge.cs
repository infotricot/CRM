namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AfterMerge : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.OrderImages", "Orderss_Id", "dbo.Orders");
            //DropIndex("dbo.OrderImages", new[] { "Orderss_Id" });
            //DropTable("dbo.OrderImages");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.OrderImages",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Link = c.String(),
            //            Orderss_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateIndex("dbo.OrderImages", "Orderss_Id");
            //AddForeignKey("dbo.OrderImages", "Orderss_Id", "dbo.Orders", "Id");
        }
    }
}
