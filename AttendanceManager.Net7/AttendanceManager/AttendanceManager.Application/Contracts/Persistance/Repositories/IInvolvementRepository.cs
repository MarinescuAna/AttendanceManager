using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IInvolvementRepository : IGenericRepository<Involvement>
    {
        IEnumerable<Involvement> GetInvolvementsByReportId(int id);
    }
}
