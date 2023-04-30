using AttendanceManager.Application.Dtos;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public sealed class GetDocumentByIdQuery : IRequest<DocumentInfoDto>
    {
        public required int Id { get; init; }
        public required Role Role { get; init; }
    }
}
