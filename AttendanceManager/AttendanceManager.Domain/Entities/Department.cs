using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceManager.Domain.Entities
{
    public class Department
    {
        public Guid DepartmentID { get; set; }
        public string Name { get; set; }
        public ICollection<Specialization> Specializations { get; set; }

    }
}
