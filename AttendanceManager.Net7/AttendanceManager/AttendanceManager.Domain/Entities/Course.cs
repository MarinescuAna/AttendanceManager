using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    /// <summary>
    /// Those courses are defined per specialization and teacher
    /// </summary>
    public class Course : EntityBase
    {
        [Key]
        public int CourseID { get; set; }
        public bool IsDeleted { get; set; } = false;
        [MaxLength(128)]
        public required string Name { get; set; }
        public required int UserSpecializationID { get; set; }
        public virtual UserSpecialization? UserSpecialization { get; set; }
        public ICollection<Document>? Documents { get; set; }
    }
}
