using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail
{
    public sealed class GetDocumentsQuery : IRequest<List<DocumentDto>>
    {
        public required string Email { get; init; }
        public required Role Role { get; init; }
    }
}
