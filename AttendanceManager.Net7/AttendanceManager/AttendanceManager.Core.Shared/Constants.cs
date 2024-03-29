﻿namespace AttendanceManager.Core.Shared
{
    public static class Constants
    {
        public static readonly string Claim_Name_Email = "email";
        public static readonly string Claim_Name_Name = "name";
        public static readonly string Claim_Name_Code = "code";
        public static readonly string Claim_Name_Role = "role";
        public static readonly string DefaultCode = "0000000000";
        public static readonly string Subject = "AttendanceManager Credentials";
        public static readonly string Body = "Hello {0}! <br><br> Your account was created using those credentials:<br><br> <strong>Email:</strong>{1} <br> <strong>Password:</strong>{2} <br><br> <strong>This email was autogenerated, please do not replay!</strong>";
       public static readonly string CharsString = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public static readonly int PasswordLength = 8;
        public static readonly int AttendanceCodeLength = 6;
        public static readonly int RefreshTokenLength = 64;
        public static readonly string DateFormat = "dddd, yyyy MMMM dd HH:mm";
        public static readonly string ShortDateFormat = "yyyy-MM-dd HH:mm";
    }

    public static class ErrorMessages
    {

        public static readonly string NoContentReportBaseMessage = "Something went wrong and the current report couldn't be loaded!";
        public static readonly string SomethingWentWrongGenericMessage = "Something went wrong! See log files to more details or try again.";
        public static readonly string SomethingWentWrongInsertBadgeMessage = "Something went wrong when the rewards was added. See the log files for more details!";
        public static readonly string SomethingWentWrongEmailSendMessage = "Something went wrong and the user will not receive any email, but the account was created. Please contact him and give him the credentials.";
    }
}