using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public sealed class Department
    {
        public required Guid DepartmentID { get; set; }
        [MaxLength(128)]
        public required string Name { get; set; }
        public required bool IsDeleted { get; set; } = false;
        public ICollection<Specialization>? Specializations { get; set; }

    }
}
