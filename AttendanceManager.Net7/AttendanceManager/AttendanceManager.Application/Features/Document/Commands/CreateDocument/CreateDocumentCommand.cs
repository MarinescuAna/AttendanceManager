using MediatR;

namespace AttendanceManager.Application.Features.Document.Commands.CreateDocument
{
    public sealed class CreateDocumentCommand:IRequest<bool>
    {
        public required string Title { get; init; }
        public required string EnrollmentYear { get; init; }
        public required string Email { get; init; }
        public required string MaxNoSeminaries { get; init; }
        public required string MaxNoLaboratories { get; init; }
        public required string MaxNoLessons { get; init; }
        public required string CourseId { get; init; }
        public required string SpecializationId { get; set; }
        public required string[] StudentIds { get; set; }
    }
}
