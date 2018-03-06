using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MoreForLess.DataAccess.Entities;

namespace MoreForLess.DataAccess.Configurations
{
    /// <summary>
    ///     Fluent Api configuration for the entity <see cref="Comment"/>.
    /// </summary>
    public class CommentConfig : EntityTypeConfiguration<Comment>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CommentConfig"/> class.
        /// </summary>
        public CommentConfig()
        {
            this.Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(c => c.Text)
                .IsRequired()
                .HasMaxLength(2000);

            this.Property(c => c.Timestamp)
                .IsRowVersion();
        }
    }
}
