using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Course.Queries.GetDashboardDataQuery
{
    public sealed class GetDashboardDataQuery : IRequest<List<CourseVm>>
    {
        public required string CurrentUserEmail { get; init; }
    }
    public sealed class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, List<CourseVm>>
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

            foreach (var course in courses)
            {
                var reports = new List<ReportDto>();
                foreach (var doc in course.Documents)
                {
                    var countedAttendances = CountAttendances(doc);
                    reports.Add(new ReportDto()
                    {
                        CreationYear = doc.CreatedOn.Year,
                        PercentageAttendances = GetPercentageAttendances(countedAttendances, doc),
                        ReportId = doc.DocumentId,
                        ReportName = doc.Title,
                        NoAttendancesAchieved = countedAttendances,
                        NoPointsAchieved = CountPoints(doc),
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
            return badges.Where(b=>!b.BadgeType.Equals(BadgeType.CustomAttendanceAchieved)&& !b.BadgeType.Equals(BadgeType.CustomBonusPointAchieved))
                .Select(b => new BadgesDto()
                    {
                        Description = b.Description!,
                        NoAchievements = b.Rewards.Count(r => r.ReportID == report.DocumentId),
                        PercentageAchievements = GetPercentageByType(report, b),
                        Role = b.UserRole,
                        Title = b.Title!,
                        BadgeType = (int)b.BadgeType,
                        ImagePath = b.ImagePath!,
                        MaxAchievements = (b.UserRole.Equals(Role.Teacher) ? 1 : 0) + GetNoUsersByRole(report, b.UserRole)
                }).ToArray();
        }
        //add current user
        private float GetPercentageByType(Domain.Entities.Document report, Domain.Entities.Badge badge) =>
            badge.Rewards.Count() == 0 ? 0 : (float)badge.Rewards.Count(r => r.ReportID == report.DocumentId) / 
                ((badge.UserRole.Equals(Role.Teacher) ? 1 : 0) + GetNoUsersByRole(report, badge.UserRole)) * 100;
        private int[] CountPoints(Domain.Entities.Document report)
        {
            var result = new int[4];
            var attendances = report.Collections!.SelectMany(s => s.Attendances!).Where(a => a.BonusPoints!=0);

            result[0] = attendances.Sum(a => a.BonusPoints);
            result[1] = attendances.Where(c => c.Collection!.CourseType.Equals(CourseType.Lecture)).Sum(a => a.BonusPoints);
            result[2] = attendances.Where(c => c.Collection!.CourseType.Equals(CourseType.Laboratory)).Sum(a => a.BonusPoints);
            result[3] = attendances.Where(c => c.Collection!.CourseType.Equals(CourseType.Seminary)).Sum(a => a.BonusPoints);
            return result;
        }
        private int[] CountAttendances(Domain.Entities.Document report)
        {
            var result = new int[4];
            var attendances = report.Collections!.SelectMany(s => s.Attendances!).Where(a=>a.IsPresent);
            
            result[0] = attendances.Count();
            result[1] = attendances.Count(a => a.Collection!.CourseType.Equals(CourseType.Lecture));
            result[2] = attendances.Count(a => a.Collection!.CourseType.Equals(CourseType.Laboratory));
            result[3] = attendances.Count(a => a.Collection!.CourseType.Equals(CourseType.Seminary));
            return result;
        }
        private int GetNoUsersByRole(Domain.Entities.Document report, Role role)
            => report.DocumentMembers!.Count(d => d.User!.Role.Equals(role));
        private float[] GetPercentageAttendances(int[] countedAttendances, Domain.Entities.Document report)
        {
            var result = new float[4];
            var noUsers = GetNoUsersByRole(report, Role.Student);
            result[0] = (float)countedAttendances[0] / ((report.MaxNoLaboratories + report.MaxNoLessons + report.MaxNoSeminaries) * noUsers) * 100;
            result[1] = report.MaxNoLessons == 0 || noUsers == 0? 0 : (float)countedAttendances[1] / (report.MaxNoLessons * noUsers) * 100;
            result[2] = report.MaxNoLaboratories == 0 || noUsers == 0 ? 0 : (float)countedAttendances[2] / (report.MaxNoLaboratories * noUsers) * 100;
            result[3] = report.MaxNoSeminaries == 0 || noUsers == 0 ? 0 : (float)countedAttendances[3] / (report.MaxNoSeminaries * noUsers) * 100;
            return result;
        }
    }
}
