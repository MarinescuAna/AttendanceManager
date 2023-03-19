using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetStudentAttendanceByDocIdAndUserId
{
    public sealed class GetStudentAttendanceByDocIdAndUserIdQuery : IRequest<StudentAttendancesDTO[]>
    {
        public required int DocumentId { get; init; }
        public required string UserId { get; init; }
    }
}
