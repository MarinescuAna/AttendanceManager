namespace AttendanceManager.Application.Modules.Authentication
{
    public sealed class GenericToken
    {
        public required string Token { get; init; }
        public required DateTime Expiration { get; init; }
    }
}
