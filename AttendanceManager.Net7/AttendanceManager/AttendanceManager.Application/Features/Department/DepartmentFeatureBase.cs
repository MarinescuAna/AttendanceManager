using AttendanceManager.Application.Contracts.Persistance;
using AutoMapper;

namespace AttendanceManager.Application.Features.Department
{
    public abstract class DepartmentFeatureBase
    {
        protected readonly IDepartmentRepository departmentRepository;
        protected readonly IMapper mapper;

        public DepartmentFeatureBase(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }
    }
}
