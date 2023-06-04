using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Features.Reward.Queries.GetAllRewardsByUserIdReportId
{
    public sealed class RewardVm
    {
        public required int BadgeId { get; init; } 
        public int? RewardId { get; init; }
        public required string ImagePath { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required BadgeType BadgeType { get; init; }
        public CourseType? ActivityType { get; init; }
        public required int? MaxNumber { get; init; }
        public required bool IsActive { get; init; } = false;
        public required bool IsCustom { get; init; }
    }
}
