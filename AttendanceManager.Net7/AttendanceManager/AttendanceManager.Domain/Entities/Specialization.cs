using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public class Specialization: EntityBase
    {
        [Key]
        public int SpecializationID { get; set; }
        [MaxLength(128)]
        public required string Name { get; set; }
        public required int DepartmentID { get; set; }
        public virtual Department? Department { get; set; }
        public ICollection<UserSpecialization>? UserSpecializations { get; set; }

    }
}
