using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface ICollectionRepository : IGenericRepository<Collection>
    {
        IQueryable<Collection> GetCollectionsByReportId(int reportId);
        void UpdateRange(List<Collection> entity);
        void DeleteCollectionsByReportId(int reportId);
    }
}
