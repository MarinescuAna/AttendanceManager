﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    public class UserSpecialization
    {
        [Key]
        public int UserSpecializationID { get; set; }
        [ForeignKey("User")]
        public required string UserID { get; set; }
        [ForeignKey("Specialization")]
        public required int SpecializationID { get; set; }
        public virtual Specialization? Specialization { get; set; }
        public virtual User? User { get; set; }
        public ICollection<Course>? Courses { get; set; }
    }
}
