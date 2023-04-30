namespace AttendanceManager.Application.Dtos
{
    public class DocumentBaseDto
    {
        public required int DocumentId { get; init; }
        public required string Title { get; init; }
        public required int EnrollmentYear { get; init; }
        public required string CourseName { get; init; }
        public required string SpecializationName { get; set; }
        public required string UpdatedOn { get; set; }
    }
}