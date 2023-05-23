using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Features.Reward.Queries.GetAllRewardsByUserIdReportId
{
    public sealed class RewardDto
    {
        public int? RewardId { get; init; }
        public required int BadgeId { get; init; }
        public required string ImagePath { get; init; }
        public required string Title { get; init; }
        public required BadgeType Type { get; init; }
        public required bool IsActive { get; init; } = false;
    }
}
