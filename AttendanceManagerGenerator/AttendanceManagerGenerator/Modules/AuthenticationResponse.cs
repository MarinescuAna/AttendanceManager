namespace AttendanceManagerGenerator.Modules
{
    public sealed class AuthenticationResponse
    {
        public string AccessToken { get; init; }
        public DateTime ExpirationDateAccessToken { get; init; }
        public string RefreshToken { get; init; }
        public DateTime ExpirationDateRefreshToken { get; init; }
    }
}
