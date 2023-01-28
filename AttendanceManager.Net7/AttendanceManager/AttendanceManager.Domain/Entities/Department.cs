using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public sealed class Department : EntityBase
    {
        [Key]
        public int DepartmentID { get; set; }
        [MaxLength(128)]
        public required string Name { get; set; }
        public ICollection<Specialization>? Specializations { get; set; }

    }
}
