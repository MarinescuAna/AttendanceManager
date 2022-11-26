using AttendanceManager.Application.CommonVms;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Commands.CreateDepartment
{
    public class CreateDepatmentCommand:IRequest<DepartmentDto>
    {
        public string Name { get; set; }
    }
}
