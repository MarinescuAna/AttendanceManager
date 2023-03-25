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
        public static readonly string Body = "Hello {0}! \n\n Your account was created using those credentials:\n\n \tEmail:{1}\n \tPassword:{2}\n\n This email was autogenerated, please do not replay!";
        public static readonly string SomethingWentWrongMessage = "Something went wrong! See log files to more details or try again.";
        public static readonly string SomethingWentWrongMessageEmailSend = "Something went wrong and the user will not receive any email, but the account was created. Please contact him and give him the credentials.";
        public static readonly string CharsString = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public static readonly int PasswordLength = 8;
        public static readonly int AttendanceCodeLength = 6;
        public static readonly string DateFormat = "dddd, dd MMMM yyyy HH:mm";
        public static readonly string ShortDateFormat = "dd.MM.yyyy HH:mm";
    }
}