using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Department.Queries.GetDepartments
{
    public sealed class GetDepartmentQuery : IRequest<DepartmentVm[]>
    {
    }
    public sealed class GetDepartmentQueryHandler : IRequestHandler<GetDepartmentQuery, DepartmentVm[]>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDepartmentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all the departments
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<DepartmentVm[]> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
            => Task.FromResult(_mapper.Map<DepartmentVm[]>(_unitOfWork.DepartmentRepository.ListAll().ToArray()));
    }
}
