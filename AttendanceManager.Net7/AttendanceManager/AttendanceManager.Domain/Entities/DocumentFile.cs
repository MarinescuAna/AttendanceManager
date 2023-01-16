using AttendanceManager.Domain.Enums;

namespace AttendanceManager.Domain.Entities
{
    public class DocumentFile
    {
        public required Guid DocumentFileID { get; init; }
        public required Guid DocumentID { get; init; }
        public required DateTime ActivityTime { get; init; }
        public required CourseType CourseType { get; init; }
        public virtual Document? Document { get; init; }
        public ICollection<Attendance>? Attendances { get; init; }
    }
}
