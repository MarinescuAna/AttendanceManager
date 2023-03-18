namespace AttendanceManager.Application.Features.AttendanceCode.Commands.CreateAttendanceCode
{
    public sealed class AttendanceCodeDTO
    {
        public required string Code { get; init; }
        public required string ExpirationDate { get; init; }

    }
}
