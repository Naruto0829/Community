namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stateupdate4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttachmentModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        HashName = c.String(),
                        FilePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.States", "latitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.States", "longitude", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Countries", "Iso3");
            DropColumn("dbo.Countries", "Iso2");
            DropColumn("dbo.Countries", "Emoji");
            DropColumn("dbo.Countries", "EmojiU");
            DropColumn("dbo.Countries", "Created_at");
            DropColumn("dbo.Countries", "Updated_at");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Countries", "Updated_at", c => c.DateTime());
            AddColumn("dbo.Countries", "Created_at", c => c.DateTime());
            AddColumn("dbo.Countries", "EmojiU", c => c.String());
            AddColumn("dbo.Countries", "Emoji", c => c.String());
            AddColumn("dbo.Countries", "Iso2", c => c.String());
            AddColumn("dbo.Countries", "Iso3", c => c.String());
            AlterColumn("dbo.States", "longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.States", "latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropTable("dbo.TagModels");
            DropTable("dbo.AttachmentModels");
        }
    }
}
