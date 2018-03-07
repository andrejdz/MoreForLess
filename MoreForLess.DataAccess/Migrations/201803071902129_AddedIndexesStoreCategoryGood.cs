using System.Data.Entity.Migrations;

namespace MoreForLess.DataAccess.Migrations
{
    /// <inheritdoc cref="DbMigration"/>
    public partial class AddedIndexesStoreCategoryGood : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.AlterColumn("dbo.Good", "IdGoodOnShop", c => c.String(nullable: false, maxLength: 25));
            this.CreateIndex("dbo.Good", "IdGoodOnShop", unique: true);
            this.CreateIndex("dbo.StoreCategory", "IdAtStore", unique: true);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.DropIndex("dbo.StoreCategory", new[] { "IdAtStore" });
            this.DropIndex("dbo.Good", new[] { "IdGoodOnShop" });
            this.AlterColumn("dbo.Good", "IdGoodOnShop", c => c.String(nullable: false));
        }
    }
}
