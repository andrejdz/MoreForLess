using System.Data.Entity.Migrations;

namespace MoreForLess.DataAccess.Migrations
{
    /// <inheritdoc cref="DbMigration"/>
    public partial class TemporaryMadeCategoryIdNullable : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.DropForeignKey("dbo.Good", "CategoryId", "dbo.Category");
            this.DropIndex("dbo.Good", new[] { "CategoryId" });
            this.AlterColumn("dbo.Good", "CategoryId", c => c.Int());
            this.CreateIndex("dbo.Good", "CategoryId");
            this.AddForeignKey("dbo.Good", "CategoryId", "dbo.Category", "Id");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.DropForeignKey("dbo.Good", "CategoryId", "dbo.Category");
            this.DropIndex("dbo.Good", new[] { "CategoryId" });
            this.AlterColumn("dbo.Good", "CategoryId", c => c.Int(nullable: false));
            this.CreateIndex("dbo.Good", "CategoryId");
            this.AddForeignKey("dbo.Good", "CategoryId", "dbo.Category", "Id", cascadeDelete: true);
        }
    }
}
