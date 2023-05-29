using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Reward.Queries.GetAllRewardsByUserIdReportId
{
    public sealed class GetAllRewardsByUserIdReportIdQuery : IRequest<List<RewardVm>>
    {
        public required int ReportId { get; set; }
        public required string Email { get; set; }
        public required Role Role { get; set; }
    }

    public sealed class GetAllRewardsByUserIdReportIdQueryHandler : IRequestHandler<GetAllRewardsByUserIdReportIdQuery, List<RewardVm>>
    {
        private readonly IReportSingleton _currentReport;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllRewardsByUserIdReportIdQueryHandler(IUnitOfWork unit, IMapper mapper, IReportSingleton currentReport)
        {
            _currentReport = currentReport;
            _unitOfWork = unit;
            _mapper= mapper;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new NoContentException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public async Task<List<RewardVm>> Handle(GetAllRewardsByUserIdReportIdQuery request, CancellationToken cancellationToken)
        {
            // get all the active badges
            var rewards = _mapper.Map<List<RewardVm>>(_unitOfWork.RewardRepository.GetRewardsAsync(r => r.UserID == request.Email && r.ReportID == _currentReport.CurrentReportInfo.ReportId));

            //get all the badges by user role
            var badges = _unitOfWork.BadgeRepository.ListAll().Where(b => b.UserRole == request.Role);

            //add inactive badges into the list
            foreach(var badge in badges)
            {
                if(!rewards.Any(r=>r.BadgeID == badge.BadgeID))
                {
                    rewards.Add(new()
                    {
                        BadgeID = badge.BadgeID,
                        ImagePath = badge.ImagePath!,
                        IsActive = false,
                        RewardId = -1,
                        Title = badge.Title!,
                        Description = badge.Description!
                    });
                }
            }

            return rewards;

        }
    }
}
