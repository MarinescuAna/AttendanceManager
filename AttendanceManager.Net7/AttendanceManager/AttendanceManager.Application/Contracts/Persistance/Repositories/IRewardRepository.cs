using AttendanceManager.Domain.Entities;
using System.Linq.Expressions;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IRewardRepository : IGenericRepository<Reward>
    {
        Task<IEnumerable<Reward>> GetRewardsAsync(Expression<Func<Reward, bool>> expression);
    }
}
