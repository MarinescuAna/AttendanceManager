namespace AttendanceManager.Application.Features.Dashboard.Queries.GetDashboardForDocumentById
{
    public sealed class DocumentDashboardDto
    {
        public StudentInteresDto[]? StudentInterests { get; set; }
    }
    public class StudentInteresDto
    {
        public required string Email { get; init; }
        public required string StudentName { get; init; }
        public required float LessonInterest { get; set; }
        public required float LaboratoryInterest { get; set; }
        public required float SeminaryInterest { get; set; }
    }

}
