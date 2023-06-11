using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Modules.Singleton;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Infrastructure.Singleton
{
    public class ReportSingleton : IReportSingleton
    {
        // UPDATE this each time when you add a new collection into this report!!
        public Dictionary<CourseType, int> LastCollectionOrder { get; set; } = new Dictionary<CourseType, int>();
        // UPDATE this each time when you add a new collection into this report!!
        public Dictionary<int, CourseType> ReportCollectionTypes { get; set; } = new Dictionary<int, CourseType>();
        public ReportDto CurrentReportInfo { get; set; }
        public Dictionary<string, Role> Members { get; set; } = new Dictionary<string, Role>();
        public void InitializeReport(Document currentReport, List<DocumentMember> members)
        {
            if (currentReport == null)
            {
                throw new Application.Exceptions.NotImplementedException(nameof(currentReport));
            }

            if (CurrentReportInfo != null)
            {
                CleanReport();
            }

            CurrentReportInfo = new ReportDto(currentReport, members.Count(m => m.User!.Role == Role.Student));

            Members = members.ToDictionary(k => k.UserID, v => v.User!.Role);

            LastCollectionOrder = new Dictionary<CourseType, int>();
            foreach (var type in Enum.GetValues(typeof(CourseType)))
            {
                var collections = currentReport.AttendanceCollections!.Where(ac => ac.CourseType.Equals(type));
                LastCollectionOrder.Add((CourseType)type, collections.Count() == 0 ? 0 : collections.Max(ac => ac.Order));
            }

            ReportCollectionTypes = currentReport.AttendanceCollections!.ToDictionary(ac => ac.AttendanceCollectionID, ac => ac.CourseType);
        }

        private void CleanReport()
        {
            CurrentReportInfo = null;
            LastCollectionOrder.Clear();
            Members.Clear();
            ReportCollectionTypes.Clear();
        }

        public void UpdateReport(Document newReport)
        {
            CurrentReportInfo.Title=newReport.Title;
            CurrentReportInfo.MaxNumberOfLectures = newReport.MaxNoLessons;
            CurrentReportInfo.MaxNumberOfLaboratories=newReport.MaxNoLaboratories;
            CurrentReportInfo.MaxNumberOfSeminaries=newReport.MaxNoSeminaries;

        }
    }
}
