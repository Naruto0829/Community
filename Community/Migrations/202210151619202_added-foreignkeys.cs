namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedforeignkeys : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Cities", "CountryId");
            CreateIndex("dbo.Cities", "StateId");
            CreateIndex("dbo.States", "CountryId");
           // AddForeignKey("dbo.Cities", "CountryId", "dbo.Countries", "Id");
            //AddForeignKey("dbo.Cities", "StateId", "dbo.States", "Id");
            //AddForeignKey("dbo.States", "CountryId", "dbo.Countries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.States", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Cities", "StateId", "dbo.States");
            DropForeignKey("dbo.Cities", "CountryId", "dbo.Countries");
            DropIndex("dbo.States", new[] { "CountryId" });
            DropIndex("dbo.Cities", new[] { "StateId" });
            DropIndex("dbo.Cities", new[] { "CountryId" });
        }
    }
}
