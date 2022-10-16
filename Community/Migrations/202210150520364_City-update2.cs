namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cityupdate2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cities", "CountryId", c => c.Int());
            AlterColumn("dbo.Cities", "StateId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cities", "StateId", c => c.Int(nullable: false));
            AlterColumn("dbo.Cities", "CountryId", c => c.Int(nullable: false));
        }
    }
}
