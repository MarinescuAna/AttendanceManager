namespace AttendanceManager.Application.Features.Attendance.Queries.GetAttendanceByAttendanceCollectionID
{
    public class StudentsAttendanceDTO
    {
        public required int AttendanceId { get; init; }
        public required string UserId { get; init; }
        public required int BonusPoints { get; init; }
        public required bool IsPresent { get; init; }
        public required string UpdatedOn { get; init; }
    }
}
