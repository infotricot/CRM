namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPosition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Position", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "Position");
        }
    }
}
