namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Country_Id", c => c.Long());
            AddColumn("dbo.Posts", "State_Id", c => c.Long());
            AlterColumn("dbo.Posts", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "UserId");
            CreateIndex("dbo.Posts", "Country_Id");
            CreateIndex("dbo.Posts", "State_Id");
            AddForeignKey("dbo.Posts", "Country_Id", "dbo.Countries", "Id");
            AddForeignKey("dbo.Posts", "State_Id", "dbo.States", "Id");
            AddForeignKey("dbo.Posts", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "State_Id", "dbo.States");
            DropForeignKey("dbo.Posts", "Country_Id", "dbo.Countries");
            DropIndex("dbo.Posts", new[] { "State_Id" });
            DropIndex("dbo.Posts", new[] { "Country_Id" });
            DropIndex("dbo.Posts", new[] { "UserId" });
            AlterColumn("dbo.Posts", "UserId", c => c.String());
            DropColumn("dbo.Posts", "State_Id");
            DropColumn("dbo.Posts", "Country_Id");
        }
    }
}
