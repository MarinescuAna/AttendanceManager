using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<List<Member>> GetMembersByReportIdAndRoleAsync(int reportId, Role? role);
        void DeleteMembersByReportId(int reportId);
        Task AddRangeAsync(List<Member> entity);
    }
}
