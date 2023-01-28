namespace AttendanceManager.Domain.Entities
{
    public class EntityBase
    {
        public required DateTime CreatedOn { get; set; }
        public required DateTime UpdatedOn { get; set; }
    }
}
