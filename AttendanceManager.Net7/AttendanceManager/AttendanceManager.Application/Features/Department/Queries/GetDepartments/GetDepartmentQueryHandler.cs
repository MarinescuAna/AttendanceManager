using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.SharedDtos;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Queries.GetDepartments
{
    public sealed class GetDepartmentQueryHandler : BaseFeature, IRequestHandler<GetDepartmentQuery, List<OrganizationDto>>
    {
        public GetDepartmentQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }
        /// <summary>
        /// Get all the organizations(department+specializations)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<OrganizationDto>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
            =>  mapper.Map<List<OrganizationDto>>(await unitOfWork.DepartmentRepository.ListAllAsync(NavigationPropertiesSetting.OnlyCollectionNavigationProps));
    }
}
