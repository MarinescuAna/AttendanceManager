using AttendanceManager.Application.SharedDtos;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.CreateDepartment
{
    public sealed class CreateDepatmentCommand:IRequest<DepartmentDto>
    {
        public required string Name { get; init; }
    }
}
