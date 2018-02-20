namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeImageOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Link = c.String(),
                        Orderss_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Orderss_Id)
                .Index(t => t.Orderss_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderImages", "Orderss_Id", "dbo.Orders");
            DropIndex("dbo.OrderImages", new[] { "Orderss_Id" });
            DropTable("dbo.OrderImages");
        }
    }
}
