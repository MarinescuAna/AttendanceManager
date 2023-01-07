using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.UpdateDepartment
{
    public sealed class UpdateOrDeleteDepartmentCommand:IRequest<bool>
    {
        public required string DepartmentId { get; init; }
        public string? Name { get; init; }

    }
}
