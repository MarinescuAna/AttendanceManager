using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    /// <summary>
    /// Attendances related to each student
    /// </summary>
    public class Involvement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvolvementID { get; set; }
        [ForeignKey("Collection")]
        public required int CollectionID { get; set; }
        [ForeignKey("User")]
        [MaxLength(254)]
        public required string UserID { get; set; }
        public string? UpdateBy { get; set; }
        public required int BonusPoints { get; set; }
        public required bool IsPresent { get; set; }
        public required DateTime UpdatedOn { get; set; }
        public virtual User? User { get; set; }
        public virtual Collection? Collection { get; set; }
    }
}
