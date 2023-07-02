using System.Text.Json.Serialization;

namespace AttendanceManagerGenerator.Modules
{
    public sealed class Configuration
    {
        [JsonPropertyName("currentUser")]
        public required User CurrentUser { get; init; }
        [JsonPropertyName("report")]
        public required Report Report { get; init; }
    }

    public sealed class User
    {
        [JsonPropertyName("email")]
        public required string Email { get; init; }
        [JsonPropertyName("password")]
        public required string Password { get; init; }
    }

    public sealed class Report
    {
        [JsonPropertyName("enrollmentYear")]
        public required int EnrollmentYear { get; init; }
        [JsonPropertyName("courseId")]
        public required int CourseId { get; init; }
        [JsonPropertyName("specializationId")]
        public required int SpecializationId { get; init; }
        [JsonPropertyName("title")]
        public string? Title { get; init; } 

    }
}
