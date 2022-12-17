using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Application.SharedDtos;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Queries.GetDepartments
{
    public sealed class GetDepartmentQueryHandler : DepartmentFeatureBase, IRequestHandler<GetDepartmentQuery, List<OrganizationDto>>
    {
        public GetDepartmentQueryHandler(IMapper mapper, IDepartmentRepository departmentRepository) : base(departmentRepository, mapper)
        {
        }

        public async Task<List<OrganizationDto>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
        {
            // Get all the organizations(department+specializations)
            var organizations = await departmentRepository.ListAllAsync();

            if (organizations == null)
            {
                return new List<OrganizationDto>();
            }

            return mapper.Map<List<OrganizationDto>>(organizations);
        }
    }
}
