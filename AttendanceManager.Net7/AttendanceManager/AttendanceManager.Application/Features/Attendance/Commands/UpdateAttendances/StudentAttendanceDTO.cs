namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateAttendances
{
    public sealed class StudentAttendanceDTO
    {
        public required int AttendanceID { get; init; }
        public required int BonusPoints { get; init; }
        public required bool IsPresent { get; init; }
    }
}
