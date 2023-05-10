using MediatR;

namespace AttendanceManager.Application.Features.Reward.Queries.GetRewardsByUserIdReportId
{
    public sealed class GetRewardsByUserIdReportIdQuery : IRequest<IEnumerable<Domain.Entities.Reward>>
    {
        public required string UserId { get; init; }
        public required int ReportId { get; init; }
}
}
