using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    public class InvolvementCode
    {
        [MaxLength(8)]
        [Key]
        public required string Code { get; set; }
        public required DateTime ExpirationDate { get; set; }
        [ForeignKey("Collection")]
        public required int CollectionID { get; set; }
        public virtual Collection? Collection { get; set; }
    }
}
