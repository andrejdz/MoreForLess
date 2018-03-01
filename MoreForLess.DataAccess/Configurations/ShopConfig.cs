using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MoreForLess.DataAccess.Entities;

namespace MoreForLess.DataAccess.Configurations
{
    /// <summary>
    ///     Fluent Api configuration for the entity <see cref="Shop"/>.
    /// </summary>
    public class ShopConfig : EntityTypeConfiguration<Shop>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShopConfig"/> class.
        /// </summary>
        public ShopConfig()
        {
            this.HasMany(s => s.Goods)
                .WithRequired()
                .HasForeignKey(g => g.ShopId);

            this.Property(s => s.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(s => s.Timestamp)
                .IsRowVersion();
        }
    }
}
