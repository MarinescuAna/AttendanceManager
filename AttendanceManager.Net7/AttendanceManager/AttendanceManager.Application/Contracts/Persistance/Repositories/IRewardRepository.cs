using AttendanceManager.Domain.Entities;
using System.Linq.Expressions;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IRewardRepository : IGenericRepository<Reward>
    {
        IQueryable<Reward> GetRewardsAsync(Expression<Func<Reward, bool>> expression);
        void DeleteRewardsByReportId(int reportId);
    }
}
