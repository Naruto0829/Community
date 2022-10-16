namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stateupdate3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Cities", new[] { "Country_Id" });
            DropIndex("dbo.Cities", new[] { "State_Id" });
            DropIndex("dbo.States", new[] { "Country_Id" });
            DropColumn("dbo.Cities", "CountryId");
            DropColumn("dbo.Cities", "StateId");
            DropColumn("dbo.States", "CountryId");
            RenameColumn(table: "dbo.Cities", name: "Country_Id", newName: "CountryId");
            RenameColumn(table: "dbo.Cities", name: "State_Id", newName: "StateId");
            RenameColumn(table: "dbo.States", name: "Country_Id", newName: "CountryId");
            AlterColumn("dbo.Cities", "CountryId", c => c.Long());
            AlterColumn("dbo.Cities", "StateId", c => c.Long());
            AlterColumn("dbo.States", "CountryId", c => c.Long());
            CreateIndex("dbo.Cities", "CountryId");
            CreateIndex("dbo.Cities", "StateId");
            CreateIndex("dbo.States", "CountryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.States", new[] { "CountryId" });
            DropIndex("dbo.Cities", new[] { "StateId" });
            DropIndex("dbo.Cities", new[] { "CountryId" });
            AlterColumn("dbo.States", "CountryId", c => c.Int());
            AlterColumn("dbo.Cities", "StateId", c => c.Int());
            AlterColumn("dbo.Cities", "CountryId", c => c.Int());
            RenameColumn(table: "dbo.States", name: "CountryId", newName: "Country_Id");
            RenameColumn(table: "dbo.Cities", name: "StateId", newName: "State_Id");
            RenameColumn(table: "dbo.Cities", name: "CountryId", newName: "Country_Id");
            AddColumn("dbo.States", "CountryId", c => c.Int());
            AddColumn("dbo.Cities", "StateId", c => c.Int());
            AddColumn("dbo.Cities", "CountryId", c => c.Int());
            CreateIndex("dbo.States", "Country_Id");
            CreateIndex("dbo.Cities", "State_Id");
            CreateIndex("dbo.Cities", "Country_Id");
        }
    }
}
