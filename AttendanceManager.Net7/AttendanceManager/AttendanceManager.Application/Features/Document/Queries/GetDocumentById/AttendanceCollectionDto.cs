namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public class AttendanceCollectionDto
    {
        public required int AttendanceCollectionId { get; init; }
        public required string ActivityTime { get; init; }
        public required string CourseType { get; init; }
    }
}