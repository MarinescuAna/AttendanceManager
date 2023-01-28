using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public sealed class GetDocumentByIdQuery : IRequest<DocumentInfoDto>
    {
        public required int Id { get; init; }
    }
}
