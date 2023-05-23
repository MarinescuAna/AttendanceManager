using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Reward.Queries.GetRewardsByUserIdReportId
{
    public sealed class GetRewardsByUserIdReportIdQuery : IRequest<IEnumerable<Domain.Entities.Reward>>
    {
        public required string UserId { get; init; }
        public required int ReportId { get; init; }
    }
    public sealed class GetRewardsByUserIdReportIdQueryHandler : BaseFeature, IRequestHandler<GetRewardsByUserIdReportIdQuery, IEnumerable<Domain.Entities.Reward>>
    {
        public GetRewardsByUserIdReportIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<IEnumerable<Domain.Entities.Reward>> Handle(GetRewardsByUserIdReportIdQuery request, CancellationToken cancellationToken)
            => await unitOfWork.RewardRepository.GetRewardsAsync(r => r.UserID!.Equals(request.UserId) && r.ReportID == request.ReportId);
    }
}
