using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MoreForLess.DataAccess.Entities;

namespace MoreForLess.DataAccess.Configurations
{
    /// <summary>
    ///     Fluent Api configuration for the entity <see cref="CategoryConfig"/>.
    /// </summary>
    class CategoryConfig : EntityTypeConfiguration<Category>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CategoryConfig"/> class.
        /// </summary>
        public CategoryConfig()
        {
            this.Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(c => c.ParentId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(c => c.Timestamp)
                .IsRowVersion();
        }
    }
}
