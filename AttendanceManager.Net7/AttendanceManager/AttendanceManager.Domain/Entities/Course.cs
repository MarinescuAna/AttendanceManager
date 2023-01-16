using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    /// <summary>
    /// Those courses are defined per specialization and teacher
    /// </summary>
    public class Course
    {
        
        public required Guid CourseID { get; set; }
        [MaxLength(128)]
        public required string Name { get; set; }
        public required bool IsDeleted { get; set; } = false;
        public required Guid SpecializationID { get; set; }
        public required string UserID { get; set; }
        public virtual User? User { get; set; }
        public virtual Specialization? Specialization { get; set; }
        public ICollection<Document>? Documents { get; set; }
    }
}
