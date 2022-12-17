namespace AttendanceManager.Application.Modules.Seed
{
    public sealed class AdminSeedSetting
    {
        public required string Fullname { get; init; }
        public required string Password { get; init; }
        public required string Code { get; init; }
        public required string Email { get; init; }

    }
}
