﻿using AttendanceManager.Application.Contracts.Infrastructure.Rewards;
using AttendanceManager.Application.Features.Attendance.Queries.HasAttendanceByUserIdCollectionId;
using AttendanceManager.Application.Features.Reward.Commands.CreateReward;
using AttendanceManager.Application.Features.Reward.Queries.GetRewardsByUserIdReportId;
using MediatR;

namespace AttendanceManager.Infrastructure.Rewards
{
    public class FirstAttendanceBadge : BaseReward
    {
        public FirstAttendanceBadge(IMediator mediator,Domain.Entities.AttendanceCollection attendanceCollection, string userId) : base(mediator,attendanceCollection, userId)
        {
        }

        public override async Task<bool> AssignBadgeAsync()
        {
            //check if this is the first attendance
            if (!await mediator.Send(new HasAttendanceByUserIdCollectionIdQuery() { CollectionId = collection.AttendanceCollectionID, StudentId = userId }))
            {
                return false;
            }

            //check if the user already have this reward
            var rewards = await mediator.Send(new GetRewardsByUserIdReportIdQuery() { ReportId = collection.DocumentID, UserId = userId });
            if (rewards.Any(r => r.Badge!.BadgeType == Domain.Enums.BadgeType.FirstAttendance))
            {
                return false;
            }

            return await mediator.Send(new CreateRewardCommand()
            {
                BadgeType = Domain.Enums.BadgeType.FirstAttendance,
                ReportId = collection.DocumentID,
                UserId = userId
            });
        }
    }
}