namespace AttendanceManager.Application.Models.Authentication
{
    public sealed class AuthenticationResponse
    {
        public required string AccessToken { get; init; }
        public required DateTime ExpirationDateAccessToken { get; init; }
        public required string RefreshToken { get; init; }
        public required DateTime ExpirationDateRefreshToken { get; init; }
    }
}
