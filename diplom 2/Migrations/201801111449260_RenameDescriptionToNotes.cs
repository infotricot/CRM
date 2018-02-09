namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameDescriptionToNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Notes", c => c.String());
            DropColumn("dbo.Contacts", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "Description", c => c.String());
            DropColumn("dbo.Contacts", "Notes");
        }
    }
}
