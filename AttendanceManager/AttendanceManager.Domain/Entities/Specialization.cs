using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceManager.Domain.Entities
{
    public class Specialization
    {
        public Guid SpecializationID { get; set; }
        public string Name { get; set; }
        public Guid DepartmentID { get; set; }
        public virtual Department Department { get; set; }
        public ICollection<UserSpecialization> UserSpecializations { get; set; }


    }
}
