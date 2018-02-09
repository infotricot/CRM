namespace diplom_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParentManager : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "ApplicationUser_Id", newName: "ParentManager_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_ApplicationUser_Id", newName: "IX_ParentManager_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_ParentManager_Id", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "ParentManager_Id", newName: "ApplicationUser_Id");
        }
    }
}
