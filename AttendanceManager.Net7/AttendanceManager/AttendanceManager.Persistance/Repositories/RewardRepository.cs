using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Persistance.Repositories
{
    public sealed class RewardRepository : GenericRepository<Reward>, IRewardRepository
    {
        public RewardRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
