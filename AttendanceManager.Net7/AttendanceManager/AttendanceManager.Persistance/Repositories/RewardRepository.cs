using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public sealed class RewardRepository : GenericRepository<Reward>, IRewardRepository
    {
        public RewardRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext,loggingService)
        {
        }

        public async Task<IEnumerable<Reward>> GetRewardsAsync(Expression<Func<Reward, bool>> expression)
            => await dbContext.Rewards.Include(r=>r.Badge).Where(expression).AsNoTracking().ToListAsync();
    }
}
