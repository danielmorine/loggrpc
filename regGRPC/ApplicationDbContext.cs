using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Extensions.FluentAPI;
using System.Threading;
using System.Threading.Tasks;

namespace regGRPC
{
    public interface IApplicationDbContext
    {
        string ConnectionString { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry Entry(object entity);
        DbSet<T> Set<T>() where T : class;
        void Dispose();
    }

    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.ConnectionString = this.Database.GetDbConnection().ConnectionString;
        }
        public string ConnectionString { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.EnvironmentTypeBuilder();

            base.OnModelCreating(builder);
        }
    }
}
