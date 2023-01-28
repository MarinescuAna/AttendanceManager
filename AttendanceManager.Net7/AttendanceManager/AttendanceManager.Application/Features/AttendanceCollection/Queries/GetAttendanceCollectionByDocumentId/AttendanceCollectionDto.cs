namespace AttendanceManager.Application.Features.AttendanceCollection.Queries.GetAttendanceCollectionByDocumentId
{
    public class AttendanceCollectionDto
    {
        public required int AttendanceCollectionId { get; init; }
        public required string ActivityTime { get; init; }
        public required string CourseType { get; init; }
    }
}