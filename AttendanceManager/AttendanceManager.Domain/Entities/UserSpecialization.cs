using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceManager.Domain.Entities
{
    public class UserSpecialization
    {
        public Guid UserSpecializationID { get; set; }
        public Guid UserID { get; set; }
        public Guid SpecializationID { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual User User { get; set; }
    }
}
