using AttendanceManager.Application.Modules.Singleton;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Contracts.Infrastructure.Singleton
{
    public interface IReportSingleton
    {
        ReportDto CurrentReportInfo { get; set; }
        Dictionary<CourseType, int> LastCollectionOrder { get; set; }
        Dictionary<int, CourseType> ReportCollectionTypes { get; set; }
        Dictionary<string, Role> Members { get; set; }
        void InitializeReport(Document currentReport, List<DocumentMember> members);
        void UpdateReport(Document newReport);
    }
}
