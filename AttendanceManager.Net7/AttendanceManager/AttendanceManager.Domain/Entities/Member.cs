using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    /// <summary>
    /// No need to inherit EntityBase  
    /// </summary>
    public class Member
    {
        [Key]
        public required Guid MemberID { get; set; }
        [ForeignKey("User")]
        [MaxLength(254)]
        public required string UserID { get; set; }
        [ForeignKey("Document")]
        public required int DocumentID { get; set; }
        public virtual User? User { get; set; }
    }
}
