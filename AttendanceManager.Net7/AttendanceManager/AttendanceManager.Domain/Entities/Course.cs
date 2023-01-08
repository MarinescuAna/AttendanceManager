using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    /// <summary>
    /// Those courses are defined per specialization
    /// </summary>
    public class Course
    {
        public required int CourseID { get; set; }
        [MaxLength(128)]
        public required string Name { get; set; }
        public required bool IsDeleted { get; set; } = false;
        public required Guid SpecializationID { get; set; }
        public virtual Specialization? Specialization { get; set; }
    }
}
