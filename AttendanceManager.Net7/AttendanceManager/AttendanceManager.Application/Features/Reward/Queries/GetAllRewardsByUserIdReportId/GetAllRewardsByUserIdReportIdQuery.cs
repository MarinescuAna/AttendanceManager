using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
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
            _mapper = mapper;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public Task<List<RewardVm>> Handle(GetAllRewardsByUserIdReportIdQuery request, CancellationToken cancellationToken)
        {
            // get all the active badges
            var rewards = _mapper.Map<List<RewardVm>>(
                _unitOfWork.RewardRepository.GetRewardsAsync(r => r.UserID == request.Email && r.ReportID == _currentReport.CurrentReportInfo.ReportId));

            //get all the badges by user role (if the current user is teacher, get the custom badges)
            var badges = _unitOfWork.BadgeRepository.ListAll().Where(b => b.UserRole == request.Role
                        || (request.Role == Role.Teacher && b.ReportID == _currentReport.CurrentReportInfo.ReportId));

            //add inactive badges into the list
            foreach (var badge in badges)
            {
                if (!rewards.Any(r => r.BadgeId == badge.BadgeID))
                {
                    rewards.Add(new()
                    {
                        BadgeId = badge.BadgeID,
                        BadgeType = badge.BadgeType,
                        ImagePath = badge.ImagePath!,
                        IsActive = false,
                        MaxNumber = badge.MaxNumber,
                        ActivityType = badge.CourseType,
                        RewardId = null,
                        Title = badge.Title!,
                        Description = badge.Description!,
                        IsCustom = badge.ReportID != null
                    });
                }
            }

            return Task.FromResult(rewards);

        }
    }
}
