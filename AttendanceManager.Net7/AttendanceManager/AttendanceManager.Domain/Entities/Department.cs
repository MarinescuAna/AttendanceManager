﻿using System.ComponentModel.DataAnnotations;

namespace AttendanceManager.Domain.Entities
{
    public class Department
    {
        public required Guid DepartmentID { get; set; }
        [MaxLength(128)]
        public required string Name { get; set; }
        public ICollection<Specialization>? Specializations { get; set; }

    }
}
