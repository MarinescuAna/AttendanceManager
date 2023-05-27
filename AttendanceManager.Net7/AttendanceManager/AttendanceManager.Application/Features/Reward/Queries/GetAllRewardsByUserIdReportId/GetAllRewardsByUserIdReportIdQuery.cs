using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Reward.Queries.GetAllRewardsByUserIdReportId
{
    public sealed class GetAllRewardsByUserIdReportIdQuery : IRequest<List<RewardDto>>
    {
        public required int ReportId { get; set; }
        public required string Email { get; set; }
        public required Role Role { get; set; }
    }

    public sealed class GetAllRewardsByUserIdReportIdQueryHandler : BaseFeature, IRequestHandler<GetAllRewardsByUserIdReportIdQuery, List<RewardDto>>
    {
        public GetAllRewardsByUserIdReportIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<List<RewardDto>> Handle(GetAllRewardsByUserIdReportIdQuery request, CancellationToken cancellationToken)
        {
            // get all the active badges
            var rewards = mapper.Map<List<RewardDto>>(await unitOfWork.RewardRepository.GetRewardsAsync(r => r.UserID == request.Email && r.ReportID == request.ReportId));

            //!! right now we filter the badges by id
            rewards.AddRange(mapper.Map<List<RewardDto>>( unitOfWork.BadgeRepository.ListAll()
                    .Where(b => !rewards.Any(r => r.BadgeID == b.BadgeID))
                    .Where(b=>b.UserRole == request.Role)
                    .ToList()
                    ));

            return rewards;

        }
    }
}
