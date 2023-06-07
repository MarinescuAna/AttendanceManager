using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Features.Badge.Queries.GetBadgePercentage
{
    public sealed class BadgeVm
    {
        public required int BadgeId { get; init; }
        public required string ImagePath { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required BadgeType BadgeType { get; init; }
        public required float Percentage { get; init; }
        public string[]? StudentsId { get; init; }
        public required bool IsCustom { get; init; }
    }
}
