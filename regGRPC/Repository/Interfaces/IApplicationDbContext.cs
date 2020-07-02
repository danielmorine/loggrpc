using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Repository.Interfaces
{
    public interface IApplicationDbContext
    {
        string ConnectionString { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry Entry(object entity);
        DbSet<T> Set<T>() where T : class;
        void Dispose();
    }
}
