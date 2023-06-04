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
        void InitializeReport(Document currentReport, int noOfStudents);
    }
}
