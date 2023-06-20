using AttendanceManager.Application.Modules.Singleton;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Contracts.Infrastructure.Singleton
{
    public interface IReportSingleton
    {
        ReportDto CurrentReportInfo { get; set; }
        Dictionary<ActivityType, int> LastCollectionOrder { get; set; }
        Dictionary<int, ActivityType> ReportCollectionTypes { get; set; }
        Dictionary<string, Role> Members { get; set; }
        void InitializeReport(Report currentReport, List<Member> members);
        void UpdateReport(Report newReport);
    }
}
