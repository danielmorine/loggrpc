using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Extensions.FluentAPI;
using System.Threading;
using System.Threading.Tasks;
using Scaffolds;

namespace regGRPC
{
    public interface IApplicationDbContext
    {
        string ConnectionString { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry Entry(object entity);
        DbSet<T> Set<T>() where T : class;
        void Dispose();

        DbSet<EnvironmentType> EnvironmentType { get; set; }
        DbSet<LevelType> LevelType { get; set; }
        DbSet<RegistrationProcess> RegistrationProcess { get; set; }
        DbSet<Report> Report { get; set; }
    }

    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.ConnectionString = this.Database.GetDbConnection().ConnectionString;
        }

        public string ConnectionString { get; set; }
        public DbSet<EnvironmentType> EnvironmentType { get; set; }
        public DbSet<LevelType> LevelType { get; set; }
        public DbSet<RegistrationProcess> RegistrationProcess { get; set; }
        public DbSet<Report> Report { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.EnvironmentTypeBuilder();
            builder.LevelTypeBuilder();
            builder.RegistrationProcessBuilder();
            builder.ReportModelBuilder();

            base.OnModelCreating(builder);
        }
    }
}
