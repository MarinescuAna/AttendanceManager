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
        public BadgeType BadgeType { get; set; }
        [MaxLength(128)]
        public string? Title { get; set; }
        [MaxLength(256)]
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        //The user's role that can achieve this badge, not the one who insert this badge
        public Role UserRole { get; set; }
        //Those to fields are used only for custom badges!!
        public int? ReportID { get; set; }
        public CourseType? CourseType { get; set; }
        public int? MaxNumber { get; set;}
        public ICollection<Reward>? Rewards { get; set; }

    }
}
