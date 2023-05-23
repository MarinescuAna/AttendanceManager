using AttendanceManager.Application.Contracts.Infrastructure.Rewards;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Infrastructure.Rewards
{
    public sealed class RewardService: IRewardService
    {
        private BaseRewardFactory _rewardFactory;

        public RewardService(BaseRewardFactory rewardFactory)
        {
            _rewardFactory = rewardFactory;
        }

        public Task<bool> AssignBadge(BadgeType badgeType, AttendanceCollection collection, string userId, Role userRole, bool commitChnages = true)
        {
            var concreteReward = _rewardFactory.CreateRewardMethod(badgeType,collection,userId);

            return concreteReward.AssignBadgeAsync(userRole,commitChnages);
        }
    }
}
