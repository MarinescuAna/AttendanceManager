namespace AttendanceManager.Application.Features.Document.Commands.CreateDocument
{
    public sealed class InsertDocumentVm
    {
        public required int DocumentId { get; set; }
        public required string UpdatedOn { get; set; }
    }
}
