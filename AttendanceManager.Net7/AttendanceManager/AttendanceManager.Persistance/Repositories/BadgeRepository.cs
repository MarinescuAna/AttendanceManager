using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.Repositories
{
    public class BadgeRepository : GenericRepository<Badge>, IBadgeRepository
    {
        public BadgeRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext, loggingService)
        {
        }

        public async Task<List<Badge>> GetUnachievedBadgesAsync(string userEmail, int reportId, Role role)
            => await dbContext.Badges.Include(a => a.Rewards)
            .Where(b => b.Rewards.Count(r => r.UserID!.Equals(userEmail) && r.ReportID == reportId) == 0 && b.UserRole.Equals(role)).ToListAsync();
    }
}
