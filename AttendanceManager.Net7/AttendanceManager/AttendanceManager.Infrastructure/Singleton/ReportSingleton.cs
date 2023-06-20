using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Modules.Singleton;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Infrastructure.Singleton
{
    public class ReportSingleton : IReportSingleton
    {
        // UPDATE this each time when you add a new collection into this report!!
        public Dictionary<ActivityType, int> LastCollectionOrder { get; set; } = new Dictionary<ActivityType, int>();
        // UPDATE this each time when you add a new collection into this report!!
        public Dictionary<int, ActivityType> ReportCollectionTypes { get; set; } = new Dictionary<int, ActivityType>();
        public ReportDto CurrentReportInfo { get; set; }
        public Dictionary<string, Role> Members { get; set; } = new Dictionary<string, Role>();
        public void InitializeReport(Report currentReport, List<Member> members)
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

            LastCollectionOrder = new Dictionary<ActivityType, int>();
            foreach (var type in Enum.GetValues(typeof(ActivityType)))
            {
                var collections = currentReport.Collections!.Where(ac => ac.ActivityType.Equals(type));
                LastCollectionOrder.Add((ActivityType)type, collections.Count() == 0 ? 0 : collections.Max(ac => ac.Order));
            }

            ReportCollectionTypes = currentReport.Collections!.ToDictionary(ac => ac.CollectionID, ac => ac.ActivityType);
        }

        private void CleanReport()
        {
            CurrentReportInfo = null;
            LastCollectionOrder.Clear();
            Members.Clear();
            ReportCollectionTypes.Clear();
        }

        public void UpdateReport(Report newReport)
        {
            CurrentReportInfo.Title=newReport.Title;
            CurrentReportInfo.MaxNumberOfLectures = newReport.MaxNoLessons;
            CurrentReportInfo.MaxNumberOfLaboratories=newReport.MaxNoLaboratories;
            CurrentReportInfo.MaxNumberOfSeminaries=newReport.MaxNoSeminaries;

        }
    }
}
