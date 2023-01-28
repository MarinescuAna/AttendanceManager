using MediatR;

namespace AttendanceManager.Application.Features.AttendanceCollection.Queries.GetAttendanceCollectionByDocumentId
{
    public sealed class GetAttendanceCollectionByDocumentIdQuery : IRequest<List<AttendanceCollectionDto>>
    {
        public required int DocumentId { get; init; }
    }
}
