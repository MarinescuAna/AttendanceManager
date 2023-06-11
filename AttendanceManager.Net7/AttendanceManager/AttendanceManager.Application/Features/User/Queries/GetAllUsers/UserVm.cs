using System.Text.Json.Serialization;

namespace AttendanceManager.Application.Features.User.Queries.GetAllUsers
{
    public sealed class UserVm
    {
        public required string Fullname { get; set; }
        [JsonPropertyName("id")]
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required int Year { get; set; }
        public required string Code { get; set; }
        public required string Updated { get; set; }
        public required string Created { get; set; }
        public required bool AccountConfirmed { get; set; }
        public required int DepartmentId { get; set; }
        public required string DepartmentName { get; set; }
        public required int[] SpecializationIds { get; set; }
    }
}