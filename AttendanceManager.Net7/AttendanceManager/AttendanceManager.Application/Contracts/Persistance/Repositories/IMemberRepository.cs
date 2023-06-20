using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<List<Member>> GetMembersByReportIdAndRoleAsync(int documentId, Role? role);
        void DeleteMembersByReportId(int documentId);
        Task AddRangeAsync(List<Member> entity);
    }
}
