using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Persistance.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext, loggingService)
        {
        }
    }
}
