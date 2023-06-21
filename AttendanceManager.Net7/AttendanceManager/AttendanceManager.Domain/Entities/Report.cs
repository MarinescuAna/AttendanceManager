using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportID { get; set; }
        [MaxLength(128)]
        public required string Title { get; set; }
        public required int EnrollmentYear { get; set; }
        public required int MaxNoSeminaries { get; set; }
        public required int MaxNoLaboratories { get; set; }
        public required int MaxNoLessons { get; set; }
        [ForeignKey("Course")]
        public required int CourseID { get; set; }
        public required DateTime CreatedOn { get; set; }
        public required DateTime UpdatedOn { get; set; }
        public required int AttendanceImportance { get; set; }
        public required int BonusPointsImportance { get; set; }
        public virtual Course? Course { get; set; }
        public ICollection<Collection>? Collections { get; set; }
        public ICollection<Member>? Members { get; set; }

    }
}
