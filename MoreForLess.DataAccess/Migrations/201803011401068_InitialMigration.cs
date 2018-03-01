using System.Data.Entity.Migrations;

namespace MoreForLess.DataAccess.Migrations
{
    /// <inheritdoc cref="DbMigration"/>
    public partial class InitialMigration : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.CreateTable(
                "dbo.Currency",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "dbo.Good",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdGoodOnShop = c.String(),
                        LinkOnProduct = c.String(nullable: false),
                        LinkOnPicture = c.String(),
                        CurrencyId = c.Int(nullable: false),
                        ShopId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currency", t => t.CurrencyId, cascadeDelete: true)
                .ForeignKey("dbo.Shop", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.CurrencyId)
                .Index(t => t.ShopId);

            this.CreateTable(
                "dbo.Shop",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.DropForeignKey("dbo.Good", "ShopId", "dbo.Shop");
            this.DropForeignKey("dbo.Good", "CurrencyId", "dbo.Currency");
            this.DropIndex("dbo.Good", new[] { "ShopId" });
            this.DropIndex("dbo.Good", new[] { "CurrencyId" });
            this.DropTable("dbo.Shop");
            this.DropTable("dbo.Good");
            this.DropTable("dbo.Currency");
        }
    }
}
