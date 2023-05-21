namespace AttendanceManager.Application.Features.Attendance.Queries.GetInvolvementsByReportId
{
    public sealed class InvolvementDto
    {
        public required int  InvolvementId { get; init; }
        public required int  CollectionId { get; init; }
        public required int  BonusPoints { get; init; }
        public required Domain.Enums.CourseType ActivityType { get; init; }
        public string? StudentEmail { get; init; }
    }
}
