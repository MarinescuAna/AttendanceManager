using AttendanceManager.Application.Dtos;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Queries.GetDepartments
{
    public sealed class GetDepartmentQuery : IRequest<DepartmentDto[]>
    {
    }
}
