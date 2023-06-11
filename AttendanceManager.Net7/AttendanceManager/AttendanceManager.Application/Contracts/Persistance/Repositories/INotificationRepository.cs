using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task AddRangeAsync(IEnumerable<Notification> entity);
    }
}
