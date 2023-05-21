using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    /// <summary>
    /// Those courses are defined per specialization and teacher
    /// </summary>
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseID { get; set; }
        public bool IsDeleted { get; set; } = false;
        [MaxLength(128)]
        public required string Name { get; set; }
        public required DateTime CreatedOn { get; set; }
        public required DateTime UpdatedOn { get; set; }
        [ForeignKey("UserSpecialization")]
        public required int UserSpecializationID { get; set; }
        public virtual UserSpecialization? UserSpecialization { get; set; }
        public ICollection<Document>? Documents { get; set; }
    }
}
