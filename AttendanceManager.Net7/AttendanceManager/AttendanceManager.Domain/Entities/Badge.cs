using AttendanceManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    public sealed class Badge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BadgeID { get; set; }
        [MaxLength(128)]
        public string? Title { get; set; }
        public string? ImagePath { get; set; }
        public BadgeType BadgeType { get; set; }
        public Role UserRole { get; set; }
        public ICollection<Reward>? Rewards { get; set;}

    }
}
