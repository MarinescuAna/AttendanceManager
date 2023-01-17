using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Features.DocumentFile.Queries.GetDocumentFileByDocumentId
{
    public class DocumentFileDto
    {
        public required string DocumentFileId { get; init; }
        public required string ActivityTime { get; init; }
        public required string CourseType { get; init; }
    }
}