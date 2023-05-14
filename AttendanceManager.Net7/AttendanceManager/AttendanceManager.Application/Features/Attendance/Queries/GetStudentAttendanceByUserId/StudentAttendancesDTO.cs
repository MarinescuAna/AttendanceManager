namespace AttendanceManager.Application.Features.Attendance.Queries.GetStudentAttendanceByUserId
{
    public sealed class StudentAttendancesDto
    {
        public required int BonusPoints { get; init; }
        public required bool IsPresent { get; init; }
        public required string CourseType { get; init; }
        public required string UserId { get; init; }
        public required string UpdatedOn { get; init; }
        public required int AttendanceId { get; init; }
    }
}
