using MediatR;

namespace AttendanceManager.Application.Features.Reward.Commands.CreateReward
{
    public sealed class CreateRewardCommand : IRequest<bool>
    {
        public required Domain.Enums.BadgeType BadgeType { get; set; }
        public required int ReportId { get; set; }
        public required string UserId { get; set; }
    }
}
