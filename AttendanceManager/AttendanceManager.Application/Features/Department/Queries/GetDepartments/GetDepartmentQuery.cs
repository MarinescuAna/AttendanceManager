using AttendanceManager.Application.CommonVms;
using MediatR;
using System.Collections.Generic;

namespace AttendanceManager.Application.Features.Department.Queries.GetDepartments
{
    public class GetDepartmentQuery: IRequest<List<DepartmentVm>>
    {
    }
}
