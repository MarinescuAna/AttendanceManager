using AttendanceManager.Application.Contracts.Infrastructure.Rewards;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Infrastructure.Rewards
{
    public class RewardFactory : BaseRewardFactory
    {
        public RewardFactory(IMediator mediator) : base(mediator)
        {
        }

        public override BaseReward CreateRewardMethod(BadgeType badgeType, AttendanceCollection collection, string userId)
             => badgeType switch
             {
                 BadgeType.FirstAttendance => new FirstAttendanceBadge(mediator,collection, userId),
                 _ => throw new NotImplementedException()
             };
    }
}
