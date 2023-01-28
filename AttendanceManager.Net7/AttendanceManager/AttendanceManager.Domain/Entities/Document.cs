using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public class Document: EntityBase
    {
        [Key]
        public int DocumentId { get; set; }
        [MaxLength(128)]
        public required string Title { get; set; }
        public required int EnrollmentYear { get; set; }
        public required int MaxNoSeminaries { get; set; }
        public required int MaxNoLaboratories { get; set; }
        public required int MaxNoLessons { get; set; }
        public required int CourseID { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual Course? Course { get; set; }
        public ICollection<AttendanceCollection>? AttendanceCollections { get; set; }
        public ICollection<DocumentMember>? DocumentMembers { get; set; }

    }
}
