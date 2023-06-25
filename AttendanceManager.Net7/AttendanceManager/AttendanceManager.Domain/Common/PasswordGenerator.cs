namespace AttendanceManager.Domain.Common
{
    public static class PasswordGenerator
    {
        public static string GeneratePassword() =>
            new(Enumerable.Repeat(Constants.CharsString, Constants.PasswordLength).Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
}
