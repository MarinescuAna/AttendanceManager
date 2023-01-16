using AttendanceManager.Application.Contracts.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Queries.GetDepartments
{
    public sealed class GetDepartmentQueryHandler : BaseFeature, IRequestHandler<GetDepartmentQuery, List<DepartmentDto>>
    {
        public GetDepartmentQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }
        /// <summary>
        /// Get all the departments
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<DepartmentDto>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
            =>  mapper.Map<List<DepartmentDto>>(await unitOfWork.DepartmentRepository.ListAllAsync());
    }
}
