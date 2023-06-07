using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Badge.Queries.GetBadgePercentage
{
    public sealed class GetBadgePercentageQuery : IRequest<List<BadgeVm>>
    {
    }

    public sealed class GetBadgePercentageQueryHandler : IRequestHandler<GetBadgePercentageQuery, List<BadgeVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportSingleton _currentReport;
        public GetBadgePercentageQueryHandler(IUnitOfWork unit, IReportSingleton currentReport)
        {
            _currentReport = currentReport;
            _unitOfWork = unit;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public Task<List<BadgeVm>> Handle(GetBadgePercentageQuery request, CancellationToken cancellationToken)
        {
            //get all badges and rewards
            var badges = _unitOfWork.BadgeRepository.ListAll()
                    .Where(b=>b.UserRole.Equals(Role.Student))
                    .Where(b => b.ReportID == null || b.ReportID == _currentReport.CurrentReportInfo.ReportId );

            var rewards = _unitOfWork.RewardRepository.ListAll()
                    .Where(r => r.ReportID == _currentReport.CurrentReportInfo.ReportId).ToList();

            var result = new List<BadgeVm>();
            foreach (var badge in badges)
            {
                var students = rewards.Where(r => r.BadgeID == badge.BadgeID).Select(r => r.UserID).ToArray();
                result.Add(new()
                {
                    BadgeId = badge.BadgeID,
                    BadgeType = badge.BadgeType,
                    Title = badge.Title!,
                    ImagePath = badge.ImagePath!,
                    IsCustom = badge.BadgeType.Equals(BadgeType.CustomAttendanceAchieved) || badge.BadgeType.Equals(BadgeType.CustomBonusPointAchieved),
                    StudentsId = students,
                    Percentage = (float)students.Count() / _currentReport.CurrentReportInfo.NoOfStudents * 100,
                    Description = badge.Description!,
                });
            }

            return Task.FromResult(result);
        }
    }
}
