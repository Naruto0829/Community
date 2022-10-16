namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categoryupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "ParentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "ParentId", c => c.String());
        }
    }
}
