using System;
using System.Collections.Generic;
using AttendanceManager.Application.Features.Department.Queries.GetDepartments;

namespace AttendanceManager.Application.SharedDtos
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<SpecializationDto> Children { get; set; }
    }
}