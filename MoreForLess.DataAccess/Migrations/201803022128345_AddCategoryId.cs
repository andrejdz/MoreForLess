using System.Data.Entity.Migrations;

namespace MoreForLess.DataAccess.Migrations
{
    /// <inheritdoc cref="DbMigration"/>
    public partial class AddCategoryId : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.AddColumn("dbo.Good", "CategoryIdOnShop", c => c.String(nullable: false));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.DropColumn("dbo.Good", "CategoryIdOnShop");
        }
    }
}
