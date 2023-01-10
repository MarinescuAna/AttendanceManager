namespace AttendanceManager.Domain.Entities
{
    public class UserSpecialization
    {
        public required Guid UserSpecializationID { get; set; }
        public required string UserID { get; set; }
        public required Guid SpecializationID { get; set; }
        public virtual Specialization? Specialization { get; set; }
        public virtual User? User { get; set; }
    }
}
