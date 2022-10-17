namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostCreatedAtUPdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "created_at", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "created_at", c => c.DateTime());
        }
    }
}
