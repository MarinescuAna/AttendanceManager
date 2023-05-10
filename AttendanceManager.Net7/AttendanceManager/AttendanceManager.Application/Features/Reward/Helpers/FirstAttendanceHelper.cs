using AttendanceManager.Application.Features.Attendance.Queries.HasAttendanceByUserIdCollectionId;
using AttendanceManager.Application.Features.Reward.Commands.CreateReward;
using AttendanceManager.Application.Features.Reward.Queries.GetRewardsByUserIdReportId;
using MediatR;

namespace AttendanceManager.Application.Features.Reward.Helpers
{
    public sealed class FirstAttendanceHelper
    {
        private IMediator _mediator;
        private int _collectionId;
        private int _reportId;
        private string _userEmail;
        public FirstAttendanceHelper(IMediator mediator, int collectionId, int reportId, string email)
        {
            _mediator = mediator;   
            _collectionId = collectionId;
            _reportId = reportId;
            _userEmail = email;
        }
        public async Task<bool> GiveReward()
        {
            //check if this is the first attendance
            if(!await _mediator.Send(new HasAttendanceByUserIdCollectionIdQuery() { CollectionId = _collectionId, StudentId = _userEmail }))
            {
                return false;
            }

            //check if the user already have this reward
            var rewards = await _mediator.Send(new GetRewardsByUserIdReportIdQuery() { ReportId = _reportId, UserId = _userEmail });
            if(rewards.Any(r=>r.Badge!.BadgeType == Domain.Enums.BadgeType.FirstAttendance))
            {
                return false;
            }

            return await _mediator.Send(new CreateRewardCommand() { 
                BadgeType = Domain.Enums.BadgeType.FirstAttendance, 
                ReportId = _reportId, 
                UserId = _userEmail
            });
        }
    }
}
