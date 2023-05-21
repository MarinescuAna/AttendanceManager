using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    public sealed class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentID { get; set; }
        [MaxLength(128)]
        public required string Name { get; set; }
        public required DateTime CreatedOn { get; set; }
        public required DateTime UpdatedOn { get; set; }
        public ICollection<Specialization>? Specializations { get; set; }

    }
}
