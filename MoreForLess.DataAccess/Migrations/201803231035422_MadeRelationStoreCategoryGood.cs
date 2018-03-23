namespace MoreForLess.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <inheritdoc cref="DbMigration"/>
    public partial class MadeRelationStoreCategoryGood : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.DropForeignKey("dbo.Good", "CategoryId", "dbo.Category");
            this.DropForeignKey("dbo.StoreCategory", "CategoryId", "dbo.Category");
            this.DropIndex("dbo.Good", new[] { "CategoryId" });
            this.DropIndex("dbo.StoreCategory", new[] { "CategoryId" });
            this.AlterColumn("dbo.Good", "CategoryIdOnShop", c => c.String(nullable: false, maxLength: 50));
            this.CreateIndex("dbo.Good", "CategoryIdOnShop");
            this.AddForeignKey("dbo.Good", "CategoryIdOnShop", "dbo.StoreCategory", "IdAtStore");
            this.DropColumn("dbo.Good", "CategoryId");
            this.DropColumn("dbo.StoreCategory", "CategoryId");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.AddColumn("dbo.StoreCategory", "CategoryId", c => c.Int());
            this.AddColumn("dbo.Good", "CategoryId", c => c.Int());
            this.DropForeignKey("dbo.Good", "CategoryIdOnShop", "dbo.StoreCategory");
            this.DropIndex("dbo.Good", new[] { "CategoryIdOnShop" });
            this.AlterColumn("dbo.Good", "CategoryIdOnShop", c => c.String(nullable: false));
            this.CreateIndex("dbo.StoreCategory", "CategoryId");
            this.CreateIndex("dbo.Good", "CategoryId");
            this.AddForeignKey("dbo.StoreCategory", "CategoryId", "dbo.Category", "Id");
            this.AddForeignKey("dbo.Good", "CategoryId", "dbo.Category", "Id");
        }
    }
}
