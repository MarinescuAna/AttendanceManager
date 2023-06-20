using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public sealed class InvolvementCode
    {
        [MaxLength(8)]
        [Key]
        public required string Code { get; set; }
        public required DateTime ExpirationDate { get; set; }
        public required int CollectionID { get; set; }
    }
}
