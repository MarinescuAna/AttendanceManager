namespace AttendanceManager.Application.Features.Attendance.Queries.GetAttendanceByAttendanceCollectionID
{
    public class StudentsAttendanceDTO
    {
        public required int AttendanceID { get; init; }
        public required string UserID { get; init; }
        public required int BonusPoints { get; init; }
        public required bool IsPresent { get; init; }
        public required string UpdatedOn { get; init; }
    }
}
