using MediatR;

namespace AttendanceManager.Application.Features.Reward.Commands.CreateReward
{
    public sealed class InsertRewardCommand : IRequest<bool>
    {
        public required int BadgeId { get; set; }
        public required int ReportId { get; set; }
        public required string UserId { get; set; }
    }
}
