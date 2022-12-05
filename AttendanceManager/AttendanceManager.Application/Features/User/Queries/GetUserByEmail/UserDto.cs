using System;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByEmail
{
    public class UserDto
    {
        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string EnrollmentYear { get; set; }
        public string Code { get; set; }
        public bool AccountConfirmed { get; set; }  
    }
}
