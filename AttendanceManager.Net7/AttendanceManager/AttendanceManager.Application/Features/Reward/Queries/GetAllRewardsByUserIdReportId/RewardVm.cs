using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Features.Reward.Queries.GetAllRewardsByUserIdReportId
{
    public sealed class RewardVm
    {
        public int? RewardId { get; init; }
        public required string ImagePath { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required BadgeID BadgeID { get; init; }
        public required bool IsActive { get; init; } = false;
    }
}
