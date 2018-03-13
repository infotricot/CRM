namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HomeWork12032018 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Processes", "IsExecuted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Processes", "IsExecuted", c => c.Boolean());
        }
    }
}
