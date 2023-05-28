using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Queries.GetDepartments
{
    public sealed class GetDepartmentQueryHandler : BaseFeature, IRequestHandler<GetDepartmentQuery, DepartmentDto[]>
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
        public Task<DepartmentDto[]> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
            =>  Task.FromResult(mapper.Map<DepartmentDto[]>(unitOfWork.DepartmentRepository.ListAll().ToArray()));
    }
}
