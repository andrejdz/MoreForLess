using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MoreForLess.DataAccess.Entities;

namespace MoreForLess.DataAccess.Configurations
{
    /// <summary>
    ///     Fluent Api configuration for the entity <see cref="Good"/>.
    /// </summary>
    public class GoodConfig : EntityTypeConfiguration<Good>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GoodConfig"/> class.
        /// </summary>
        public GoodConfig()
        {
            this.Property(g => g.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(g => g.Price)
                .IsRequired();

            this.Property(g => g.LinkOnProduct)
                .IsRequired();

            this.Property(g => g.LinkOnPicture)
                .IsOptional();

            this.Property(g => g.Timestamp)
                .IsRowVersion();
        }
    }
}
