namespace AttendanceManager.Application.Features.Dashboard.Queries.GetDashboardForDocumentById
{
    public sealed class DocumentDashboardDto
    {
        public DocumentDashboardItemsDto[]? StudentInterests { get; set; }
        public DailyActivityDto[]? AttendancePercentage { get; set; }
    }
    public class DocumentDashboardItemsDto
    {
        public required string Email { get; init; }
        public required string StudentName { get; init; }
        public required float LessonValue { get; set; }
        public required float LaboratoryValue { get; set; }
        public required float SeminaryValue { get; set; }
    }

    public class DailyActivityDto{
        public required int AttendanceCollectionId { get; init; }
        public required string Datetime { get; init; }
        public required float Percentage { get; init; }
        public required string CourseType { get; init; }
    }
}
