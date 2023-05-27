using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Contracts.Infrastructure.Rewards
{
    public abstract class BaseRewardFactory
    {
        protected IMediator mediator;
        public BaseRewardFactory(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public abstract BaseReward CreateRewardMethod(BadgeID badgeType, AttendanceCollection collection, string userId);
    }
}
