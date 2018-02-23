namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableExecuteDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Processes", "ExecuteDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Processes", "ExecuteDate", c => c.DateTime(nullable: false));
        }
    }
}
