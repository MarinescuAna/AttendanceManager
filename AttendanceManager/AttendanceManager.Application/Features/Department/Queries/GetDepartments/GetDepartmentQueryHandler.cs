using AttendanceManager.Application.CommonVms;
using AttendanceManager.Application.Contracts.Persistance;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceManager.Application.Features.Department.Queries.GetDepartments
{
    public class GetDepartmentQueryHandler : DepartmentFeatureBase, IRequestHandler<GetDepartmentQuery, List<DepartmentVm>>
    {
        public GetDepartmentQueryHandler(IMapper mapper, IDepartmentRepository departmentRepository):base(departmentRepository,mapper)
        {
        }

        public async Task<List<DepartmentVm>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
        {
            var departments = await departmentRepository.ListAllAsync();

            if (departments == null)
            { 
                return new List<DepartmentVm>(); 
            }

            return mapper.Map<List<DepartmentVm>>(departments);
        }
    }
}
