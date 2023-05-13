namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateStudentsInvolvement
{
    public sealed class StudentInvolvementDto
    {
        public required int AttendanceID { get; init; }
        public required int BonusPoints { get; init; }
        public required bool IsPresent { get; init; }
    }
}
