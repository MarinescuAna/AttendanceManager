using MediatR;
using System;

namespace AttendanceManager.Application.Features.Specialization.Commands.CreateSpecialization
{
    public class CreateSpecializationCommand:IRequest<SpecializationDto>
    {
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
