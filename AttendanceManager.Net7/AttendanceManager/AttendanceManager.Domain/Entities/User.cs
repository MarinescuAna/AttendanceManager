using AttendanceManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public sealed class User: EntityBase
    {
        [MaxLength(254)]
        [Key]
        public required string Email { get; set; }
        [MaxLength(128)]
        public required string FullName { get; set; }
        public required Role Role { get; set; }
        [MaxLength(64)]
        public string? Password { get; set; }
        public int EnrollmentYear { get; set; } = 0;
        // This code represents the unique code that will appear instead of names for students 
        [MaxLength(32)]
        public required string Code { get; set; }
        [MaxLength(128)]
        public string? RefreshToken { get; set; }
        public DateTime? ExpRefreshToken { get; set; }
        public required bool AccountConfirmed { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<UserSpecialization>? UserSpecializations { get; set; }
        public ICollection<DocumentMember>? DocumentMembers { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
    }
}
