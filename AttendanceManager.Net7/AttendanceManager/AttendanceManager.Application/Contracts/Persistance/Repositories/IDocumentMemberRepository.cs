using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IDocumentMemberRepository : IGenericRepository<DocumentMember>
    {
        Task<List<DocumentMember>> GetDocumentMembersByDocumentIdAndRoleAsync(int documentId, Role? role);
        void DeleteMembersByDocumentId(int documentId);
    }
}
