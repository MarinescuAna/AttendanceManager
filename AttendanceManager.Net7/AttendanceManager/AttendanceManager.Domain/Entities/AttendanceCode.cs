using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public sealed class AttendanceCode
    {
        [Key]
        public int AttendanceCodeID { get; set; }
        public required string Code { get; set; }
        public required DateTime ExpirationDate { get; set; }
    }
}
