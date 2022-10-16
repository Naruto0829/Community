namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeforeignkey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cities", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Cities", "StateId", "dbo.States");
            DropForeignKey("dbo.States", "CountryId", "dbo.Countries");
            DropIndex("dbo.Cities", new[] { "CountryId" });
            DropIndex("dbo.Cities", new[] { "StateId" });
            DropIndex("dbo.States", new[] { "CountryId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.States", "CountryId");
            CreateIndex("dbo.Cities", "StateId");
            CreateIndex("dbo.Cities", "CountryId");
            AddForeignKey("dbo.States", "CountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.Cities", "StateId", "dbo.States", "Id");
            AddForeignKey("dbo.Cities", "CountryId", "dbo.Countries", "Id");
        }
    }
}
