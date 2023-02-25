using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Commands.UpdateAttendances
{
    public sealed class UpdateAttendancesCommand: IRequest<bool>
    {
        public required StudentAttendanceDTO[] Students { get; init; }
    }
}
