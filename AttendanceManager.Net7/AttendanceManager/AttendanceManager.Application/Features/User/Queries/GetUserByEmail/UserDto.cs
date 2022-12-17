namespace AttendanceManager.Application.Features.User.Queries.GetUserByEmail
{
    public sealed class UserDto
    {
        public required Guid UserID { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required string Password { get; set; }
        public required string EnrollmentYear { get; set; }
        public required string Code { get; set; }
        public required bool AccountConfirmed { get; set; }
    }
}
