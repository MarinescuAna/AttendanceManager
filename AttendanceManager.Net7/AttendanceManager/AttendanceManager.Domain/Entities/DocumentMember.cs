using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    /// <summary>
    /// No need to inherit EntityBase  
    /// </summary>
    public class DocumentMember
    {
        [Key]
        public int DocumentMemberID { get; set; }
        public required string UserID { get; set; }
        public required int DocumentID { get; set; }
        public virtual User? User { get; set; }
        public virtual Document? Document { get; set; }
    }
}
