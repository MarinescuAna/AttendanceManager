namespace AttendanceManager.Domain.Entities
{
    public class Document
    {
        public required Guid DocumentId { get; init; }
        public required string Title { get; init; }
        public required int EnrollmentYear { get; init; }
        //TODO remove this because there is already an user id saved in course id and those courses are already per user
        public required string UserID { get; init; }
        public required bool IsDeleted { get; init; }
        public required DateTime CreationDate { get; init; }
        public required DateTime UpdateDate { get; init; }
        public required int MaxNoSeminaries { get; init; }
        public required int MaxNoLaboratories { get; init; }
        public required int MaxNoLessons { get; init; }
        public required Guid CourseID { get; init; }
        public required Guid SpecializationID { get; set; }
        public virtual User? User { get; init; }
        public virtual Course? Course { get; init; }
        public virtual Specialization? Specialization { get; init; }
        public ICollection<UserDocument>? UserDocuments { get; set; }
        public ICollection<DocumentFile>? DocumentFiles { get; set; }

    }
}
