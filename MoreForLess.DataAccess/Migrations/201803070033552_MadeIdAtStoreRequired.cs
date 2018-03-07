using System.Data.Entity.Migrations;

namespace MoreForLess.DataAccess.Migrations
{
    /// <inheritdoc cref="DbMigration"/>
    public partial class MadeIdAtStoreRequired : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.AlterColumn("dbo.Good", "IdGoodOnShop", c => c.String(nullable: false));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.AlterColumn("dbo.Good", "IdGoodOnShop", c => c.String());
        }
    }
}
