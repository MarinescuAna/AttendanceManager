using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public class Specialization
    {
        public required Guid SpecializationID { get; set; }
        [MaxLength(128)]
        public required string Name { get; set; }
        public required bool IsDeleted { get; set; } = false;

        public required Guid DepartmentID { get; set; }
        public virtual Department? Department { get; set; }
        public ICollection<UserSpecialization>? UserSpecializations { get; set; }


    }
}
