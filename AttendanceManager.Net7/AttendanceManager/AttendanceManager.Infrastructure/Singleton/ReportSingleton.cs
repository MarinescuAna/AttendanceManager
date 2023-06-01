using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Modules.Singleton;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using System;

namespace AttendanceManager.Infrastructure.Singleton
{
    public class ReportSingleton : IReportSingleton
    {
        // UPDATE this each time when you add a new collection into this report!!
        public Dictionary<CourseType, int> LastCollectionOrder { get; set; } = new Dictionary<CourseType, int>();
        // UPDATE this each time when you add a new collection into this report!!
        public Dictionary<int, CourseType> ReportCollectionTypes { get; set; } = new Dictionary<int, CourseType>();
        public ReportDto? CurrentReportInfo { get; set; }
        public void InitializeReport(Document currentReport)
        {
            if (currentReport == null)
            {
                throw new Application.Exceptions.NotImplementedException(nameof(currentReport));
            }

            if (CurrentReportInfo != null)
            {
                CleanReport();
            }

            CurrentReportInfo = new ReportDto(currentReport);

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
            ReportCollectionTypes.Clear();
        }
    }
}
