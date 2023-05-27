using AttendanceManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    public class Reward
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RewardID { get; set; }
        [ForeignKey("Badge")]
        public BadgeID BadgeID { get; set; }
        [ForeignKey("User")]
        [MaxLength(254)]
        public string? UserID { get; set; }
        [ForeignKey("Report")]
        public int ReportID { get; set; }
        public virtual Document? Report { get; set; }
        public virtual User? User { get; set; }
        public virtual Badge? Badge { get; set; }

    }
}
