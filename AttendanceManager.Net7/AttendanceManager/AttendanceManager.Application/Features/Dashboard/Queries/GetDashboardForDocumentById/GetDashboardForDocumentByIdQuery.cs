using MediatR;

namespace AttendanceManager.Application.Features.Dashboard.Queries.GetDashboardForDocumentById
{
    public sealed class GetDashboardForDocumentByIdQuery : IRequest<DocumentDashboardDto>
    {
        public required int DocumentId { get; init; }
    }
}
