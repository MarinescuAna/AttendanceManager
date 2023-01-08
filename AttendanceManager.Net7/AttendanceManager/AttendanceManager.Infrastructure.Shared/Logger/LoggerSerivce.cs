using Serilog;

namespace AttendanceManager.Infrastructure.Shared.Logger
{
    public static class LoggerSerivce
    {
        public static void LogException(Exception exception, string? methodName)
        {
            Log.Error(string.Format(Constants.Info_LogError,methodName));
            Log.Error(string.Format(Constants.MessageReceived_LogError, exception.Message));
            if (!string.IsNullOrEmpty(exception.InnerException?.ToString()))
            {
                Log.Error(string.Format(Constants.InnerMessageReceived_LogError,exception.InnerException));
            }
        }
    }
}
