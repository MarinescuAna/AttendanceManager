using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IAttendanceCollectionRepository : IGenericRepository<AttendanceCollection>
    {
        Task<List<AttendanceCollection>> GetAttendanceCollectionsByDocumentIdAsync(int documentId);
        Task<AttendanceCollection> GetAttendanceCollectionByIdAsync(int id);
        bool HasAttendanceByReportIdUserId(int reportId, string email);
    }
}
