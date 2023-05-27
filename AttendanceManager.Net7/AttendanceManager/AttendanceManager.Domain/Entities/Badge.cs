using AttendanceManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public sealed class Badge
    {
        [Key]
        public BadgeID BadgeID { get; set; }
        [MaxLength(128)]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public Role UserRole { get; set; }
        public ICollection<Reward>? Rewards { get; set; }

    }
}
