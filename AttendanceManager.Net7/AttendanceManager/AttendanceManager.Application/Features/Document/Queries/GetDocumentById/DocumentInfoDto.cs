namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public sealed class DocumentInfoDto
    {
        public required int DocumentId { get; init; }
        public required string Title { get; init; }
        public required int EnrollmentYear { get; init; }
        public required int MaxNoSeminaries { get; init; }
        public required int MaxNoLaboratories { get; init; }
        public required int MaxNoLessons { get; init; }
        public required int NoSeminaries { get; init; }
        public required int NoLaboratories { get; init; }
        public required int NoLessons { get; init; }
        public required int CourseId { get; init; }
        public required string CourseName { get; init; }
        public required int SpecializationId { get; set; }
        public required string SpecializationName { get; set; }
        public required string CreationDate { get; init; }
        public required string UpdateDate { get; init; }
        public required string CreatedBy { get; init; }
    }
}