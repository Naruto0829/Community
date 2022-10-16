namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Country : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        CountryCode = c.String(),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        flag = c.Int(nullable: false),
                        wikiDataId = c.String(),
                        CountryId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        Country_Id = c.Long(),
                        State_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .ForeignKey("dbo.States", t => t.State_Id)
                .Index(t => t.Country_Id)
                .Index(t => t.State_Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Iso3 = c.String(),
                        NumericCode = c.String(),
                        Iso2 = c.String(),
                        Phonecode = c.String(),
                        Capital = c.String(),
                        Currency = c.String(),
                        CurrencyName = c.String(),
                        CurrencySymbol = c.String(),
                        Tld = c.String(),
                        Native = c.String(),
                        Region = c.String(),
                        Subregion = c.String(),
                        Timezones = c.String(),
                        Translations = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        Emoji = c.String(),
                        EmojiU = c.String(),
                        Created_at = c.DateTime(nullable: false),
                        Updated_at = c.DateTime(nullable: false),
                        Flag = c.Int(nullable: false),
                        WikiDataId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        country_code = c.String(),
                        fips_code = c.String(),
                        iso2 = c.String(),
                        type = c.String(),
                        latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        flag = c.Int(nullable: false),
                        wikiDataId = c.String(),
                        CountryId = c.Int(nullable: false),
                        Country_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .Index(t => t.Country_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.States", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Cities", "State_Id", "dbo.States");
            DropForeignKey("dbo.Cities", "Country_Id", "dbo.Countries");
            DropIndex("dbo.States", new[] { "Country_Id" });
            DropIndex("dbo.Cities", new[] { "State_Id" });
            DropIndex("dbo.Cities", new[] { "Country_Id" });
            DropTable("dbo.States");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
        }
    }
}
