using System.Text.Json.Serialization;

namespace AttendanceManager.Application.Features.Course.Queries.GetCoursesQuery
{
    public sealed class CoursesDto
    {
        [JsonPropertyName("id")]
        public required string CourseId { get; init; }
        public required string Name { get; init; }
        //TODO load this only if is needed
        //public required string SpecializationId { get; init; }
        public required string SpecializationName { get; init; }
    }
}
