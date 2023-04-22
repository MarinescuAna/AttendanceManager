using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public sealed class AttendanceCode
    {
        [Key]
        public int AttendanceCodeID { get; set; }
        [MaxLength(8)]
        public required string Code { get; set; }
        public required DateTime ExpirationDate { get; set; }
        public required int AttendanceCollectionId { get; set; }
    }
}
