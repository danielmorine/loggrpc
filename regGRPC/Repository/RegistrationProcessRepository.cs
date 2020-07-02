using Repository.Interfaces;
using Scaffolds;

namespace Repository
{
    public interface IRegistrationProcessRepository : IRepositoryBase<RegistrationProcess> { }
    public class RegistrationProcessRepository : RepositoryBase<RegistrationProcess>, IRegistrationProcessRepository
    {
        public RegistrationProcessRepository(IApplicationDbContext db) : base(db) { }
    }
}
