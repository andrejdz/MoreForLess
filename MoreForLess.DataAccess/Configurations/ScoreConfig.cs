using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MoreForLess.DataAccess.Entities;

namespace MoreForLess.DataAccess.Configurations
{
    /// <summary>
    ///     Fluent Api configuration for the entity <see cref="ScoreConfig"/>.
    /// </summary>
    public class ScoreConfig : EntityTypeConfiguration<Score>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScoreConfig"/> class.
        /// </summary>
        public ScoreConfig()
        {
            this.Property(s => s.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(s => s.Value)
                .IsRequired();

            this.Property(s => s.Timestamp)
                .IsRowVersion();
        }
    }
}
