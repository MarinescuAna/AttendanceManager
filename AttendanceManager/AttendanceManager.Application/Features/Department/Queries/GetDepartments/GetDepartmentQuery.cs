using AttendanceManager.Application.SharedDtos;
using MediatR;
using System.Collections.Generic;

namespace AttendanceManager.Application.Features.Department.Queries.GetDepartments
{
    public class GetDepartmentQuery: IRequest<List<OrganizationDto>>
    {
    }
}
