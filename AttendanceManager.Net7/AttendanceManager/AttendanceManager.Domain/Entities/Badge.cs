using AttendanceManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public sealed class Badge
    {
        [Key]
        public int BadgeID { get; set; }
        [MaxLength(128)]
        public string? Title { get; set; }
        public BadgeType BadgeType { get; set; }
        public ICollection<Reward>? Rewards { get; set;}

    }
}
