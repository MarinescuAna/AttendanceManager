using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public class UserSpecialization
    {
        [Key]
        public int UserSpecializationID { get; set; }
        public required string UserID { get; set; }
        public required int SpecializationID { get; set; }
        public virtual Specialization? Specialization { get; set; }
        public virtual User? User { get; set; }
        public ICollection<Course>? Courses { get; set; }
    }
}
