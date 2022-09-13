using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceManager.Domain.Entities
{
    public class Specialisation
    {
        public Guid SpecialisationID { get; set; }
        public string Name { get; set; }
        public Guid DepartmentID { get; set; }
        public virtual Department Department { get; set; }
        public ICollection<UserSpecialisation> UserSpecialisations { get; set; }


    }
}
