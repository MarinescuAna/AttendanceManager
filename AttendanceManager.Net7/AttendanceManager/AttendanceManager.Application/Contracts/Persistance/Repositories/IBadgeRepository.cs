using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IBadgeRepository: IGenericRepository<Badge>
    {
        Task<List<Badge>> GetUnachievedBadgesAsync(string userEmail, int reportId, Role role);
        void DeleteCustomBadgesByReportId(int reportId);
        Task<List<Badge>> GetBadgesForReportAsync(int reportId);
    }
}
