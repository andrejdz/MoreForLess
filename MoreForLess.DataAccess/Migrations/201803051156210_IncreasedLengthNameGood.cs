using System.Data.Entity.Migrations;

namespace MoreForLess.DataAccess.Migrations
{
    /// <inheritdoc cref="DbMigration"/>
    public partial class IncreasedLengthNameGood : DbMigration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.AlterColumn("dbo.Good", "Name", c => c.String(nullable: false, maxLength: 1000));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.AlterColumn("dbo.Good", "Name", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
