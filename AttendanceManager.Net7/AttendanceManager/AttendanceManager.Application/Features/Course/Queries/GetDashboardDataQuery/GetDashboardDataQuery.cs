using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Course.Queries.GetDashboardDataQuery
{
    public sealed class GetDashboardDataQuery : IRequest<List<CourseVm>>
    {
        public required string CurrentUserEmail { get; init; }
    }
    public sealed class GetDashboardDataQueryHandler: IRequestHandler<GetDashboardDataQuery, List<CourseVm>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDashboardDataQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CourseVm>> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
        {
            var courses = await _unitOfWork.CourseRepository.GetCoursesForDashboardAsync(request.CurrentUserEmail);
            var result = new List<CourseVm>();

            foreach(var course in courses)
            {
                var reports = new List<ReportDto>();
                foreach(var doc in course.Documents)
                {
                    reports.Add(new ReportDto()
                    {
                        CreationYear = doc.CreatedOn.Year,
                        NoPossibleAttendances = GetPossibleAttendances(doc),
                        NoStudents = GetNoUsersByRole(doc, Role.Student),
                        ReportId = doc.DocumentId,
                        ReportName = doc.Title,
                        NoPointsAchieved = CountAttendances(doc),
                        NoAttendancesAchieved = CountPoints(doc),
                        Badges = await GetBadgesAsync(doc)
                    });
                }
                result.Add(new()
                {
                    CourseId = course.CourseID,
                    CourseName = course.Name,
                    Reports = reports.ToArray(),
                });
            }
            return result;
        }
        private async Task<BadgesDto[]> GetBadgesAsync(Domain.Entities.Document report)
        {
            var badges = await _unitOfWork.BadgeRepository.GetBadgesForReportAsync(report.DocumentId);
            return badges.Select(b => new BadgesDto()
            {
                BadgeType = (int)b.BadgeType,
                Description = b.Description,
                NoAchievements = b.Rewards.Count(r=>r.ReportID == report.DocumentId),
                NoPossibleAchievements = GetNoUsersByRole(report,b.UserRole),
                Role = b.UserRole,
                Title = b.Title
            }).ToArray();
        }
        private int CountPoints(Domain.Entities.Document report)
            => report.AttendanceCollections!.SelectMany(s => s.Attendances!).Sum(a => a.BonusPoints);
        private int CountAttendances(Domain.Entities.Document report)
            => report.AttendanceCollections!.SelectMany(s => s.Attendances!).Count(a => a.IsPresent);
        private int GetNoUsersByRole(Domain.Entities.Document report, Role role)
            => report.DocumentMembers!.Count(d => d.User!.Role.Equals(role));
        private int GetPossibleAttendances(Domain.Entities.Document report) 
            => (report.MaxNoLaboratories + report.MaxNoLessons + report.MaxNoSeminaries) * GetNoUsersByRole(report,Role.Student);
    }
}
