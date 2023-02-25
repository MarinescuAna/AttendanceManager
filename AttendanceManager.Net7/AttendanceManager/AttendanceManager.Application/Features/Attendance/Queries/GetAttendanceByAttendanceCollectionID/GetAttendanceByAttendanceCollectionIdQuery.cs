using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetAttendanceByAttendanceCollectionID
{
    public sealed class GetAttendanceByAttendanceCollectionIdQuery:IRequest<List<StudentsAttendanceDTO>>
    {
        public required int AttendanceCollectionId { get; init; }
    }
}
