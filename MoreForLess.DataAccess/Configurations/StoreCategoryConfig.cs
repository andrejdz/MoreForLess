using System.ComponentModel.DataAnnotations.Schema;
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
            this.Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnOrder(0);

            this.Property(c => c.IdAtStore)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(c => c.ParentIdAtStore)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(c => c.Timestamp)
                .IsRowVersion();
        }
    }
}
