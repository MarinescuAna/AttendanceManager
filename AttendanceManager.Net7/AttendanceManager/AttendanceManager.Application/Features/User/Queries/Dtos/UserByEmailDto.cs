namespace AttendanceManager.Application.Features.User.Queries.Dtos
{
    public sealed class UserByEmailDto : BaseUserDto
    {
        public required string Password { get; set; }
        public required bool AccountConfirmed { get; set; }

    }
}
