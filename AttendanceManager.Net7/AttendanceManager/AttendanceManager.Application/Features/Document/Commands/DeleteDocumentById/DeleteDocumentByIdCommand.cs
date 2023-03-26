using MediatR;

namespace AttendanceManager.Application.Features.Document.Commands.DeleteDocumentById
{
    public sealed class DeleteDocumentByIdCommand : IRequest<bool>
    {
        public required int DocumentId { get; init; }
    }
}
