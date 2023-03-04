namespace AttendanceManager.Application.Features.Attendance.Queries.GetTotalAttendancesByDocumentId
{
    public sealed class TotalAttendanceDTO
    {
        public required string UserID { get; init; }
        public required string UserName { get; init; }
        public required string Code { get; init; }
        public required int BonusPoints { get; init; }
        public required int CourseAttendances { get; init; }
        public required int LaboratoryAttendances { get; init; }
        public required int SeminaryAttendances { get; init; }
    }
}
