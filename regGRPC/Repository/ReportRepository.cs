using Repository.Interfaces;
using Scaffolds;

namespace Repository
{
    public interface IReportRepository : IRepositoryBase<Report> { }
    public class ReportRepository : RepositoryBase<Report>, IReportRepository
    {
        public ReportRepository(IApplicationDbContext db) : base(db) { }
    }
}
