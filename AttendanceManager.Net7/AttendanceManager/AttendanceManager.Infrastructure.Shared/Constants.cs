using System;

namespace AttendanceManager.Infrastructure.Shared
{
    public static class Constants
    {
        public readonly static string Info_LogError = "An error occures when we try to call {0}.";
        public readonly static string MessageReceived_LogError = "Message received: {0}";
        public readonly static string InnerMessageReceived_LogError = "Inner exception received: {0}";
    }
}