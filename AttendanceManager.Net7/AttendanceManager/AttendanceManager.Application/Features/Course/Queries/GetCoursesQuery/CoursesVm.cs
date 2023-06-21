namespace AttendanceManager.Application.Features.Course.Queries.GetCoursesQuery
{
    public sealed class CoursesVm
    {
        public required string CourseId { get; init; }
        public required string Name { get; init; }
        public required string UpdatedOn { get; init; }
        public required string SpecializationId { get; init; }
        public required string SpecializationName { get; init; }
        public required int ReportsLinked { get; init; }
    }
}
