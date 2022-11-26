using AttendanceManager.Domain.Enums;
using System;
using System.Collections.Generic;

namespace AttendanceManager.Domain.Entities
{
    public class User
    {
        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }
        public string EnroleYear { get; set; }
        // This code represents the unique code that will appear instead of names for students 
        public string Code { get; set; }
        public ICollection<UserSpecialization> UserSpecialisations { get; set; }
    }
}
