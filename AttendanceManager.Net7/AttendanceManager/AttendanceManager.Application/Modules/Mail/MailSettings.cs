namespace AttendanceManager.Application.Models.Mail
{
    public sealed class MailSettings
    {
        public required string Host { get; init; }
        public required int Port { get; init; }
        public required string User { get; init; }
        public required string Password { get; init; }
        public required string From { get; init; }
        public required bool UseSSL { get; init; }
        public required bool UseStartTls { get; init; }
    }
}
