using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationID { get; set; }
        [ForeignKey("User")]
        [MaxLength(254)]
        public required string UserID { get; set; }
        [MaxLength(254)]
        public required string Message { get; set; }
        public required bool IsRead { get; set; }
        public required DateTime CreatedOn { get; set; }
        public virtual User? User { get; set; }
    }
}
