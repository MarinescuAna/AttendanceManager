namespace AttendanceManager.Application.Features.User.Queries.Dtos
{
    public class BaseUserDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required string Code { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpRefreshToken { get; set; }
    }
}
