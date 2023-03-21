using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface IDocumentMemberRepository : IGenericRepository<DocumentMember>
    {
        Task<List<DocumentMember>> GetDocumentMembersByDocumentIdAndRoleAsync(int documentId, Role? role);
        Task<DocumentMember?> GetMemberByDocumentIdAndUserIdAsync(int documentId, string userId);
    }
}
