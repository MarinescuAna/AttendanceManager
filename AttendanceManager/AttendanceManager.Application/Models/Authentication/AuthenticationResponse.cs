using System;

namespace AttendanceManager.Application.Models.Authentication
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
