using AttendanceManager.Application.Features.Reward.Queries.GetRewardsByUserIdReportId;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Contracts.Infrastructure.Rewards
{
    public abstract class BaseReward
    {
        protected AttendanceCollection collection;
        protected string userId;
        protected IMediator mediator;

        public BaseReward(IMediator mediator, AttendanceCollection attendanceCollection, string userId)
        {
            collection = attendanceCollection;
            this.userId = userId;
            this.mediator = mediator;
        }

        public abstract Task<bool> AssignBadgeAsync(Role userRole, bool commitChnages = true);

        public async Task<bool> HasAlreadyTheBadge(BadgeID type)
        {
            //check if the user already have this reward
            var rewards = await mediator.Send(new GetRewardsByUserIdReportIdQuery() { ReportId = collection.DocumentID, UserId = userId });
            return rewards.Any(r => r.Badge!.BadgeID == type);
        }
    }
}
