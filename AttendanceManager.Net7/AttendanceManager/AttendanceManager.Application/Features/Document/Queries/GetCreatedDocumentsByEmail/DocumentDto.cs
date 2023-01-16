namespace AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail
{
    public sealed class DocumentDto
    {
        public required string DocumentId { get; init; }
        public required string Title { get; init; }
        public required string EnrollmentYear { get; init; }
        public required string CourseName { get; init; }
        public required string SpecializationName { get; set; }
    }
}