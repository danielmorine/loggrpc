using Repository.Interfaces;
using Scaffolds;

namespace Repository
{
    public interface IEnvironmentTypeRepository : IRepositoryBase<EnvironmentType> { }

    public class EnvironmentTypeRepository : RepositoryBase<EnvironmentType>, IEnvironmentTypeRepository
    {
        public EnvironmentTypeRepository(IApplicationDbContext db) : base(db) { }
    }
}
