using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetTotalAttendancesByDocumentId
{
    public sealed class GetTotalAttendancesByDocumentIdQuery : IRequest<List<TotalAttendanceDTO>>
    {
        public required int DocumentId { get; init; }
    }
}
