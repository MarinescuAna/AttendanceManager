using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    public class Specialization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpecializationID { get; set; }
        [MaxLength(128)]
        public required string Name { get; set; }
        public required DateTime UpdatedOn { get; set; }
        public required int DepartmentID { get; set; }
    }
}
