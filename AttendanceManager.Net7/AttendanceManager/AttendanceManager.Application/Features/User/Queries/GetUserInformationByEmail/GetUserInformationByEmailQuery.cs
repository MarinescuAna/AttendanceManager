using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetUserInformationByEmail
{
    public sealed class GetUserInformationByEmailQuery : IRequest<UserVm>
    {
        public required string Email { get; init; }

    }
    public sealed class GetUserInformationByEmailQueryHandler : IRequestHandler<GetUserInformationByEmailQuery, UserVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserInformationByEmailQueryHandler( IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserVm> Handle(GetUserInformationByEmailQuery request, CancellationToken cancellationToken)
        {
            var userSpecializations = await _unitOfWork.UserSpecializationRepository.GetUserSpecializationsByExpression(u => u.UserID == request.Email)
                ?? throw new NotFoundException(nameof(User), request.Email);

            var specializations = _mapper.Map<SpecializationDto[]>(userSpecializations);

            var userDepartment = await _unitOfWork.DepartmentRepository.GetAsync(d => userSpecializations.FirstOrDefault()!.Specialization!.DepartmentID == d.DepartmentID);

            return new()
            {
                DepartmentID = userDepartment!.DepartmentID,
                DepartmentName = userDepartment!.Name,
                Specializations = specializations
            };
        }
    }
}
