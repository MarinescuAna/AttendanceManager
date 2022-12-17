namespace AttendanceManager.Application.Models.Authentication
{
    public sealed class AuthenticationResponse
    {
        public required string Token { get; init; }
        public string? RefreshToken { get; init; }
    }
}
