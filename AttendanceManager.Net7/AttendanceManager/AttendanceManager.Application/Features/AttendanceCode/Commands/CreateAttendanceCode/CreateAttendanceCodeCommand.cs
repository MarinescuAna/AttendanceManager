using MediatR;

namespace AttendanceManager.Application.Features.AttendanceCode.Commands.CreateAttendanceCode
{
    public sealed class CreateAttendanceCodeCommand : IRequest<AttendanceCodeDTO>
    {
        public required int Minutes { get; init; }
    }
}
