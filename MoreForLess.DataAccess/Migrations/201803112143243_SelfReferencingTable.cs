using System.Data.Entity.Migrations;

namespace MoreForLess.DataAccess.Migrations
{
    /// <inheritdoc cref="DbMigration"/>
    public partial class SelfReferencingTable : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.DropIndex("dbo.StoreCategory", new[] { "IdAtStore" });
            this.DropPrimaryKey("dbo.StoreCategory");
            this.AlterColumn("dbo.StoreCategory", "ParentIdAtStore", c => c.String(maxLength: 50));
            this.AddPrimaryKey("dbo.StoreCategory", "IdAtStore");
            this.CreateIndex("dbo.StoreCategory", "ParentIdAtStore");
            this.AddForeignKey("dbo.StoreCategory", "ParentIdAtStore", "dbo.StoreCategory", "IdAtStore");
            this.DropColumn("dbo.StoreCategory", "Id");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.AddColumn("dbo.StoreCategory", "Id", c => c.Int(nullable: false, identity: true));
            this.DropForeignKey("dbo.StoreCategory", "ParentIdAtStore", "dbo.StoreCategory");
            this.DropIndex("dbo.StoreCategory", new[] { "ParentIdAtStore" });
            this.DropPrimaryKey("dbo.StoreCategory");
            this.AlterColumn("dbo.StoreCategory", "ParentIdAtStore", c => c.String(nullable: false, maxLength: 50));
            this.AddPrimaryKey("dbo.StoreCategory", "Id");
            this.CreateIndex("dbo.StoreCategory", "IdAtStore", unique: true);
        }
    }
}
