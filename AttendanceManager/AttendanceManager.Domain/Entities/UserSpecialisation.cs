using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceManager.Domain.Entities
{
    public class UserSpecialisation
    {
        public Guid UserSpecialisationID { get; set; }
        public Guid UserID { get; set; }
        public Guid SpecialisationID { get; set; }
        public virtual Specialisation Specialisation { get; set; }
        public virtual User User { get; set; }
    }
}
