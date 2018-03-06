using System.Data.Entity.Migrations;

namespace MoreForLess.DataAccess.Migrations
{
    /// <inheritdoc cref="DbMigration"/>
    public partial class AddedNewTables : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        ParentId = c.String(nullable: false, maxLength: 50),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);

            this.CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 2000),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        GoodId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Good", t => t.GoodId)
                .Index(t => t.GoodId);

            this.CreateTable(
                "dbo.Score",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        GoodId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Good", t => t.GoodId)
                .Index(t => t.GoodId);

            this.CreateTable(
                "dbo.StoreCategory",
                c => new
                    {
                        IdAtStore = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 250),
                        ParentIdAtStore = c.String(nullable: false, maxLength: 50),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ShopId = c.Int(nullable: false),
                        CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.IdAtStore)
                .ForeignKey("dbo.Category", t => t.CategoryId)
                .ForeignKey("dbo.Shop", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.ShopId)
                .Index(t => t.CategoryId);

            this.AddColumn("dbo.Good", "CategoryId", c => c.Int(nullable: false));
            this.CreateIndex("dbo.Good", "CategoryId");
            this.AddForeignKey("dbo.Good", "CategoryId", "dbo.Category", "Id", cascadeDelete: true);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.DropForeignKey("dbo.StoreCategory", "ShopId", "dbo.Shop");
            this.DropForeignKey("dbo.StoreCategory", "CategoryId", "dbo.Category");
            this.DropForeignKey("dbo.Score", "GoodId", "dbo.Good");
            this.DropForeignKey("dbo.Comment", "GoodId", "dbo.Good");
            this.DropForeignKey("dbo.Good", "CategoryId", "dbo.Category");
            this.DropIndex("dbo.StoreCategory", new[] { "CategoryId" });
            this.DropIndex("dbo.StoreCategory", new[] { "ShopId" });
            this.DropIndex("dbo.Score", new[] { "GoodId" });
            this.DropIndex("dbo.Comment", new[] { "GoodId" });
            this.DropIndex("dbo.Good", new[] { "CategoryId" });
            this.DropColumn("dbo.Good", "CategoryId");
            this.DropTable("dbo.StoreCategory");
            this.DropTable("dbo.Score");
            this.DropTable("dbo.Comment");
            this.DropTable("dbo.Category");
        }
    }
}
