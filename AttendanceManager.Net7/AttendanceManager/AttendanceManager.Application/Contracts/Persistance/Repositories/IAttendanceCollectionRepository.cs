using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IAttendanceCollectionRepository : IGenericRepository<AttendanceCollection>
    {
        IQueryable<AttendanceCollection> GetCollectionsByReportId(int reportId);
        void UpdateRange(List<AttendanceCollection> entity);
        void DeleteCollectionsByReportId(int reportId);
    }
}
