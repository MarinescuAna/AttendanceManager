using AttendanceManager.Domain.Entities;
using MediatR;

namespace AttendanceManager.Application.Contracts.Infrastructure.Rewards
{
    public abstract class BaseReward
    {
        protected AttendanceCollection collection;
        protected string userId;
        protected IMediator mediator;

        public BaseReward(IMediator mediator,AttendanceCollection attendanceCollection, string userId)
        {
            collection = attendanceCollection;
            this.userId = userId;    
            this.mediator = mediator;
        }

        public abstract Task<bool> AssignBadgeAsync();
    }
}
