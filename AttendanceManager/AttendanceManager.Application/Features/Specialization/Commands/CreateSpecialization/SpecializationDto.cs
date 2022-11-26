using System;

namespace AttendanceManager.Application.Features.Specialization.Commands.CreateSpecialization
{
    public class SpecializationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
