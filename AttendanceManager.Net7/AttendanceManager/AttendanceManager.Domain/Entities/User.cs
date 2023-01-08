using AttendanceManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public sealed class User
    {
        public required Guid UserID { get; set; }
        [MaxLength(128)]
        public required string FullName { get; set; }
        [MaxLength(254)]
        public required string Email { get; set; }
        public required Role Role { get; set; }
        [MaxLength(64)]
        public string? Password { get; set; }
        public int? EnrollmentYear { get; set; }
        // This code represents the unique code that will appear instead of names for students 
        //[MaxLength(16)]
        public string? Code { get; set; }
        public required DateTime Created { get; set; }
        public required DateTime Updated { get; set; }
        public required bool AccountConfirmed { get; set; }
        public ICollection<UserSpecialization>? UserSpecializations { get; set; }
    }
}
