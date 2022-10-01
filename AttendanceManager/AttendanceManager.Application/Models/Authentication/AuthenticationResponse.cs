using System;

namespace AttendanceManager.Application.Models.Authentication
{
    public class AuthenticationResponse
    {
        public Guid UserID { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Code { get; set; }
        public string Token { get; set; }
    }
}
