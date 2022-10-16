namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cityupdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Cities", "CountryCode");
            DropColumn("dbo.Cities", "Latitude");
            DropColumn("dbo.Cities", "Longitude");
            DropColumn("dbo.Cities", "created_at");
            DropColumn("dbo.Cities", "updated_at");
            DropColumn("dbo.Cities", "flag");
            DropColumn("dbo.Cities", "wikiDataId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cities", "wikiDataId", c => c.String());
            AddColumn("dbo.Cities", "flag", c => c.Int(nullable: false));
            AddColumn("dbo.Cities", "updated_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.Cities", "created_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.Cities", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Cities", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Cities", "CountryCode", c => c.String());
        }
    }
}
