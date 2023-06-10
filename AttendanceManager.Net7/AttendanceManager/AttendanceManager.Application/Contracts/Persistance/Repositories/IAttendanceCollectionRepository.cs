using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IAttendanceCollectionRepository : IGenericRepository<AttendanceCollection>
    {
        IQueryable<AttendanceCollection> GetCollectionsByReportId(int reportId);
        int GetLastOrder(int reportId, CourseType type);
        void UpdateRange(List<AttendanceCollection> entity);
    }
}
