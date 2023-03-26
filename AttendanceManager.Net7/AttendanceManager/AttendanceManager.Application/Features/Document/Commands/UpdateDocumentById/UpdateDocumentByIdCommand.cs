using MediatR;

namespace AttendanceManager.Application.Features.Document.Commands.UpdateDocumentById
{
    public sealed class UpdateDocumentByIdCommand : IRequest<bool>
    {
        public required int DocumentId { get; init; }
        public required string Title { get; init; }
        public required int NoSeminaries { get; init; }
        public required int NoLaboratories { get; init; }
        public required int NoLessons { get; init; }
        public required int CourseId { get; init; }
        public required int AttendanceImportance { get; init; }
        public required int BonusPointsImportance { get; init; }
    }
}
