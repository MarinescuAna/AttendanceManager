using AttendanceManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    /// <summary>
    /// No need to inherit EntityBase  
    /// </summary>
    public class DocumentMember
    {
        [Key]
        public int DocumentMemberID { get; set; }
        [ForeignKey("User")]
        public required string UserID { get; set; }
        [ForeignKey("Document")]
        public required int DocumentID { get; set; }
        public required DocumentRole Role { get; set; } = DocumentRole.Member; 
        public virtual User? User { get; set; }
        public virtual Document? Document { get; set; }
    }
}
