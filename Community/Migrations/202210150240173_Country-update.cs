namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Countryupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Countries", "Created_at", c => c.DateTime());
            AlterColumn("dbo.Countries", "Updated_at", c => c.DateTime());
            AlterColumn("dbo.Countries", "Flag", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Countries", "Flag", c => c.Int(nullable: false));
            AlterColumn("dbo.Countries", "Updated_at", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Countries", "Created_at", c => c.DateTime(nullable: false));
        }
    }
}
