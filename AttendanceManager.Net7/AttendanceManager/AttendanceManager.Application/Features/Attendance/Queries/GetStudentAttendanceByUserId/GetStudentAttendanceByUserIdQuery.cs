using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetStudentAttendanceByUserId
{
    public sealed class GetStudentAttendanceByUserIdQuery : IRequest<StudentAttendancesDTO[]>
    {
        public required string UserId { get; init; }
    }
}
