using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByEmail
{
    public class UserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }
        public string EnroleYear { get; set; }
        public string Code { get; set; }
    }
}
