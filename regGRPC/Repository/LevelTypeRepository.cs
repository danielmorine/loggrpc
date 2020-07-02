using Repository.Interfaces;
using Scaffolds;

namespace Repository
{
    public interface ILevelTypeRepository : IRepositoryBase<LevelType> { }

    public class LevelTypeRepository : RepositoryBase<LevelType>, ILevelTypeRepository
    {
        public LevelTypeRepository(IApplicationDbContext db) : base (db) { }
    }
}
