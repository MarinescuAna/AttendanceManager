namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public sealed class DocumentInfoDto
    {
        public required string Title { get; init; }
        public required string EnrollmentYear { get; init; }
        public required string Email { get; init; }
        public required string MaxNoSeminaries { get; init; }
        public required string MaxNoLaboratories { get; init; }
        public required string MaxNoLessons { get; init; }
        public required string CourseId { get; init; }
        public required string CourseName { get; init; }
        public required string SpecializationId { get; set; }
        public required string SpecializationName { get; set; }
        public required string CreationDate { get; init; }
        public required string UpdateDate { get; init; }
    }
}