using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.HasAttendanceByUserIdCollectionId
{
    public sealed class HasAttendanceByUserIdCollectionIdQuery:IRequest<bool>
    {
        public required int CollectionId { get; set; }
        public required string StudentId { get; set; }
    }
}
