using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Domain.Common;
using Serilog;

namespace AttendanceManager.Infrastructure.Logging
{
    public sealed class LoggingService:ILoggingService
    {
        public void LogException(Exception exception, string? methodName)
        {
            Log.Error(string.Format(ErrorMessages.Info_LogError, methodName));
            Log.Error(string.Format(ErrorMessages.MessageReceived_LogError, exception.Message));
            if (!string.IsNullOrEmpty(exception.InnerException?.ToString()))
            {
                Log.Error(string.Format(ErrorMessages.InnerMessageReceived_LogError, exception.InnerException));
            }
        }
    }

}
