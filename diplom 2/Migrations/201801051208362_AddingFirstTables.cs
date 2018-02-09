namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFirstTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FIO = c.String(),
                        Phones = c.String(),
                        Email = c.String(),
                        Skype = c.String(),
                        Viber = c.String(),
                        Facebook = c.String(),
                        Description = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        Counterparty_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Counterparties", t => t.Counterparty_Id)
                .Index(t => t.Counterparty_Id);
            
            CreateTable(
                "dbo.Counterparties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        City = c.String(),
                        Address = c.String(),
                        Requisites = c.String(),
                        Web = c.String(),
                        CounterpartyType = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        LastModify = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Processes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreateDate = c.DateTime(nullable: false),
                        ExecuteDate = c.DateTime(nullable: false),
                        PlaningDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Counterparty_Id = c.Int(),
                        Manager_Id = c.String(maxLength: 128),
                        ProcessType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Counterparties", t => t.Counterparty_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Manager_Id)
                .ForeignKey("dbo.ProcessTypes", t => t.ProcessType_Id)
                .Index(t => t.Counterparty_Id)
                .Index(t => t.Manager_Id)
                .Index(t => t.ProcessType_Id);
            
            CreateTable(
                "dbo.ProcessTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceUrl = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ChangeDate = c.DateTime(nullable: false),
                        ReadyDate = c.DateTime(nullable: false),
                        Counterparty_Id = c.Int(),
                        Manager_Id = c.String(maxLength: 128),
                        StatusOrder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Counterparties", t => t.Counterparty_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Manager_Id)
                .ForeignKey("dbo.StatusOrders", t => t.StatusOrder_Id)
                .Index(t => t.Counterparty_Id)
                .Index(t => t.Manager_Id)
                .Index(t => t.StatusOrder_Id);
            
            CreateTable(
                "dbo.ProductInOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Size = c.String(),
                        Color = c.String(),
                        Price = c.String(),
                        Comments = c.String(),
                        Order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.StatusOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserCounterparties",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Counterparty_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Counterparty_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Counterparties", t => t.Counterparty_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Counterparty_Id);
            
            AddColumn("dbo.AspNetUsers", "Phone", c => c.String());
            AddColumn("dbo.AspNetUsers", "City", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "StatusOrder_Id", "dbo.StatusOrders");
            DropForeignKey("dbo.ProductInOrders", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Manager_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "Counterparty_Id", "dbo.Counterparties");
            DropForeignKey("dbo.Processes", "ProcessType_Id", "dbo.ProcessTypes");
            DropForeignKey("dbo.Processes", "Manager_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Processes", "Counterparty_Id", "dbo.Counterparties");
            DropForeignKey("dbo.ApplicationUserCounterparties", "Counterparty_Id", "dbo.Counterparties");
            DropForeignKey("dbo.ApplicationUserCounterparties", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Contacts", "Counterparty_Id", "dbo.Counterparties");
            DropIndex("dbo.ApplicationUserCounterparties", new[] { "Counterparty_Id" });
            DropIndex("dbo.ApplicationUserCounterparties", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProductInOrders", new[] { "Order_Id" });
            DropIndex("dbo.Orders", new[] { "StatusOrder_Id" });
            DropIndex("dbo.Orders", new[] { "Manager_Id" });
            DropIndex("dbo.Orders", new[] { "Counterparty_Id" });
            DropIndex("dbo.Processes", new[] { "ProcessType_Id" });
            DropIndex("dbo.Processes", new[] { "Manager_Id" });
            DropIndex("dbo.Processes", new[] { "Counterparty_Id" });
            DropIndex("dbo.Contacts", new[] { "Counterparty_Id" });
            DropColumn("dbo.AspNetUsers", "City");
            DropColumn("dbo.AspNetUsers", "Phone");
            DropTable("dbo.ApplicationUserCounterparties");
            DropTable("dbo.StatusOrders");
            DropTable("dbo.ProductInOrders");
            DropTable("dbo.Orders");
            DropTable("dbo.ProcessTypes");
            DropTable("dbo.Processes");
            DropTable("dbo.Counterparties");
            DropTable("dbo.Contacts");
        }
    }
}
