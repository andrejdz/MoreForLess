using System.Data.Entity.Migrations;

namespace MoreForLess.DataAccess.Migrations
{
    /// <inheritdoc cref="DbMigration"/>
    public partial class AddedIdToStoreCategory : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.DropPrimaryKey("dbo.StoreCategory");
            this.AddColumn("dbo.StoreCategory", "Id", c => c.Int(nullable: false, identity: true));
            this.AddPrimaryKey("dbo.StoreCategory", "Id");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.DropPrimaryKey("dbo.StoreCategory");
            this.DropColumn("dbo.StoreCategory", "Id");
            this.AddPrimaryKey("dbo.StoreCategory", "IdAtStore");
        }
    }
}
