using System.Data.Entity.ModelConfiguration;
using MoreForLess.DataAccess.Entities;

namespace MoreForLess.DataAccess.Configurations
{
    /// <summary>
    ///     Fluent Api configuration for the entity <see cref="StoreCategory"/>.
    /// </summary>
    public class StoreCategoryConfig : EntityTypeConfiguration<StoreCategory>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StoreCategoryConfig"/> class.
        /// </summary>
        public StoreCategoryConfig()
        {
            this.HasKey(c => c.IdAtStore);

            this.Property(c => c.IdAtStore)
                .IsRequired()
                .HasMaxLength(50);

            this.HasMany(c => c.Goods)
                .WithRequired()
                .HasForeignKey(g => g.CategoryIdOnShop)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(p => p.ParentIdAtStore);

            this.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(c => c.ParentIdAtStore)
                .IsOptional()
                .HasMaxLength(50);

            this.Property(c => c.Timestamp)
                .IsRowVersion();
        }
    }
}
