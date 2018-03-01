using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MoreForLess.DataAccess.Entities;

namespace MoreForLess.DataAccess.Configurations
{
    /// <summary>
    ///     Fluent Api configuration for the entity <see cref="Currency"/>.
    /// </summary>
    public class CurrencyConfig : EntityTypeConfiguration<Currency>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyConfig"/> class.
        /// </summary>
        public CurrencyConfig()
        {
            this.HasMany(c => c.Goods)
                .WithRequired()
                .HasForeignKey(g => g.CurrencyId);

            this.Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(c => c.Timestamp)
                .IsRowVersion();
        }
    }
}
