using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface IDocumentMemberRepository : IGenericRepository<DocumentMember>
    {
        Task<List<User>> GetStudentsByDocumentIdAsync(int documentId);
        Task<DocumentMember?> GetMemberByDocumentIdAndUserIdAsync(int documentId, string userId);
    }
}
