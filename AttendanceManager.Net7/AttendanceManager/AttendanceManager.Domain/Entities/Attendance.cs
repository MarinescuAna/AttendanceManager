using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    /// <summary>
    /// No need to inherit EntityBase 
    /// Attendances related to each student
    /// </summary>
    public class Attendance
    {
        [Key]
        public int AttendanceID { get; set; }
        public required int AttendanceCollectionID { get; set; }
        public required string UserID { get; set; }
        public virtual User? User { get; set; }
        public virtual AttendanceCollection? AttendanceCollection { get; set; }
    }
}
