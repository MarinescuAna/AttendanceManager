namespace AttendanceManager.Application.Models.Authentication
{
    public sealed class AuthenticationRequest
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
