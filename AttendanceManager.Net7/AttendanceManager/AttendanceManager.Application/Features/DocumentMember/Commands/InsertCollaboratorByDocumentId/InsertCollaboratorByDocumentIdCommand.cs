using AttendanceManager.Application.SharedDtos;
using MediatR;

namespace AttendanceManager.Application.Features.DocumentMember.Commands.InsertCollaboratorByDocumentId
{
    public sealed class InsertCollaboratorByDocumentIdCommand : IRequest<DocumentMembersDto>
    {
        public required string Email { get; init; }
        public required int DocumentId { get; init; }
    }
}
