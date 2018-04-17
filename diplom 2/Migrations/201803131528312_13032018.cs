namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13032018 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Processes", "IsExecuted", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Processes", "IsExecuted");
        }
    }
}
