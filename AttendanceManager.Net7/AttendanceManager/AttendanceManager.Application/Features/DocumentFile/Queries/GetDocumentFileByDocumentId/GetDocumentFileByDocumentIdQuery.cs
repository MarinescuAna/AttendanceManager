using MediatR;

namespace AttendanceManager.Application.Features.DocumentFile.Queries.GetDocumentFileByDocumentId
{
    public sealed class GetDocumentFileByDocumentIdQuery : IRequest<List<DocumentFileDto>>
    {
        public required string DocumentId { get; init; }
    }
}
