namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stateupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.States", "CountryId", c => c.Int());
            DropColumn("dbo.States", "fips_code");
            DropColumn("dbo.States", "iso2");
            DropColumn("dbo.States", "type");
            DropColumn("dbo.States", "created_at");
            DropColumn("dbo.States", "updated_at");
            DropColumn("dbo.States", "flag");
            DropColumn("dbo.States", "wikiDataId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.States", "wikiDataId", c => c.String());
            AddColumn("dbo.States", "flag", c => c.Int(nullable: false));
            AddColumn("dbo.States", "updated_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.States", "created_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.States", "type", c => c.String());
            AddColumn("dbo.States", "iso2", c => c.String());
            AddColumn("dbo.States", "fips_code", c => c.String());
            AlterColumn("dbo.States", "CountryId", c => c.Int(nullable: false));
        }
    }
}
