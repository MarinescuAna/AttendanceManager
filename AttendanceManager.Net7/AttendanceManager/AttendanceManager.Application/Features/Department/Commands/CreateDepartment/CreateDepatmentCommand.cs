using AttendanceManager.Application.SharedDtos;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.CreateDepartment
{
    public sealed class CreateDepatmentCommand:IRequest<Guid>
    {
        public required string Name { get; init; }
    }
}
