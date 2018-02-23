namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExecuteDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Processes", "ExecuteDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Processes", "ExecuteDescription");
        }
    }
}
