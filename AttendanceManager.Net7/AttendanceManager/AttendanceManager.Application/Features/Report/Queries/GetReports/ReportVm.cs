namespace AttendanceManager.Application.Features.Report.Queries.GetReports
{
    public sealed class ReportVm
    {
        public required int DocumentId { get; init; }
        public required string Title { get; init; }
        public required int EnrollmentYear { get; init; }
        public required string CourseName { get; init; }
        public required string SpecializationName { get; set; }
        public required string UpdatedOn { get; set; }
        public bool IsCreator { get; set; } = false;
    }
}
