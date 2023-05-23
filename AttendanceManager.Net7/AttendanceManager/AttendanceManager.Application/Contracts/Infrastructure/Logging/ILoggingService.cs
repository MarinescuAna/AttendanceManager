namespace AttendanceManager.Application.Contracts.Infrastructure.Logging
{
    public interface ILoggingService
    {
        void LogException(Exception exception, string? methodName);
    }
}
