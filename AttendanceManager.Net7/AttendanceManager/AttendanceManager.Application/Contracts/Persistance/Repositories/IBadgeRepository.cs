using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IBadgeRepository: IGenericRepository<Badge>
    {
        Task<List<Badge>> GetUnachievedBadgesAsync(string userEmail, int reportId);
    }
}
