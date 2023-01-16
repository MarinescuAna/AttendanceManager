namespace AttendanceManager.Domain.Entities
{
    public class Attendance
    {
        public required int AttendanceID { get; init; }
        public required Guid DocumentFileID { get; init; }
        public required string UserID { get; init; }
        public virtual User? User { get; init; }
        public virtual DocumentFile? DocumentFile { get; init; }
    }
}
