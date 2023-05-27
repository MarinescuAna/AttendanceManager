namespace AttendanceManager.Application.Features.Attendance.Queries.GetInvolvementsByReportId
{
    public sealed class InvolvementVm
    {
        public required int  InvolvementId { get; init; }
        public required int  CollectionId { get; init; }
        public required int  BonusPoints { get; init; }
        public required Domain.Enums.CourseType ActivityType { get; init; }
        public required string Student { get; init; }
        public required string Email { get; init; }
        public bool IsPresent { get;init; }
        public string? UpdateOn { get;init; }
    }
}
