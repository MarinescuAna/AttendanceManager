using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateAttendanceByCodeAndAttendanceId
{
    public sealed class UpdateAttendanceByCodeAndAttendanceIdCommand : IRequest<bool>
    {
        public required string AttendanceCode { get; init; }
        public required int AttendanceId { get; init; }
        public required int AttendanceCollectionId { get; init; }
    }
}
