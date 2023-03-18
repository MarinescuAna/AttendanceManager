using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    public sealed class AttendanceCode
    {
        [Key]
        public int AttendanceCodeID { get; set; }
        public required string Code { get; set; }
        public required DateTime ExpirationDate { get; set; }
        public required int AttendanceCollectionId { get; set; }
    }
}
