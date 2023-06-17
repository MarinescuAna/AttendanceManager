using AttendanceManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    /// <summary>
    /// Collection of attendances
    /// </summary>
    public class Collection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollectionID { get; set; }
        [ForeignKey("Document")]
        public required int DocumentID { get; set; }
        public required int Order { get; set; }
        [MaxLength(128)]
        public string? Title { get; set; }
        public required DateTime HeldOn { get; set; }
        public required CourseType CourseType { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
    }
}
