namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Counterparties", "Region", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Counterparties", "Region");
        }
    }
}
