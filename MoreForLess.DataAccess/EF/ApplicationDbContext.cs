using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MoreForLess.DataAccess.Configurations;
using MoreForLess.DataAccess.Entities;
using MoreForLess.DataAccess.Migrations;

namespace MoreForLess.DataAccess.EF
{
    /// <summary>
    /// Class <see cref="ApplicationDbContext"/> is inherted from class <see cref="DbContext"/>
    /// and provides access to users' database.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        public ApplicationDbContext()
            : base("MoreForLessDb")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>("MoreForLessDb"));
        }

        public virtual DbSet<Good> Goods { get; set; }

        public virtual DbSet<Currency> Currencies { get; set; }

        public virtual DbSet<Shop> Shops { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new GoodConfig());
            modelBuilder.Configurations.Add(new CurrencyConfig());
            modelBuilder.Configurations.Add(new ShopConfig());
        }
    }
}
