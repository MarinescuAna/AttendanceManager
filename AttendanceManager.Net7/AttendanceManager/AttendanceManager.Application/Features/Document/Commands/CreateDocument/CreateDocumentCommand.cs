using MediatR;

namespace AttendanceManager.Application.Features.Document.Commands.CreateDocument
{
    public sealed class CreateDocumentCommand:IRequest<InsertDocumentDto>
    {
        public required string Title { get; init; }
        public required int EnrollmentYear { get; init; }
        public string? Email { get; set; }
        public required int MaxNoSeminaries { get; init; }
        public required int MaxNoLaboratories { get; init; }
        public required int MaxNoLessons { get; init; }
        public required int CourseId { get; init; }
        public required int SpecializationId { get; set; }
        public required int AttendanceImportance { get; init; }
        public required int BonusPointsImportance { get; init; }
        public required string[] StudentIds { get; set; }

    }
}
