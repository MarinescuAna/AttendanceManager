using AttendanceManager.Application.SharedDtos;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Queries.GetDepartments
{
    public sealed class GetDepartmentQuery : IRequest<List<OrganizationDto>>
    {
    }
}
