using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface IAttendanceCollectionRepository: IGenericRepository<AttendanceCollection>
    {
        Task<List<AttendanceCollection>> GetAttendanceCollectionsByDocumentIdAsync(int documentId);
    }
}
