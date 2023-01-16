namespace AttendanceManager.Domain.Entities
{
    public class UserDocument
    {
        public required Guid UserDocumentId { get; init; }
        public required string UserID { get; init; }
        public required Guid DocumentID { get; init; }
        public virtual User? User { get; init; }
        public virtual Document? Document { get; init; }
    }
}
