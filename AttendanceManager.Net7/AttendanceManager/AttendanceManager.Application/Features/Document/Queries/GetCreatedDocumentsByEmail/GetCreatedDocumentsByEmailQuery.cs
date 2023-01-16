using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail
{
    public sealed class GetCreatedDocumentsByEmailQuery : IRequest<List<DocumentDto>>
    {
        public required string Email { get; init; }
    }
}
