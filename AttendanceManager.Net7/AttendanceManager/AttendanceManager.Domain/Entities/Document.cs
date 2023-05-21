using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentId { get; set; }
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
        public bool IsDeleted { get; set; } = false;
        public virtual Course? Course { get; set; }
        public ICollection<AttendanceCollection>? AttendanceCollections { get; set; }
        public ICollection<DocumentMember>? DocumentMembers { get; set; }
        public ICollection<Reward>? Rewards { get; set; }

    }
}
