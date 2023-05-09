using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetUserInformationByEmail
{
    public sealed class GetUserInformationByEmailQueryHandler : BaseFeature, IRequestHandler<GetUserInformationByEmailQuery, UserInfoDto>
    {
        public GetUserInformationByEmailQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<UserInfoDto> Handle(GetUserInformationByEmailQuery request, CancellationToken cancellationToken)
        {
            var userSpecializations = await unitOfWork.UserSpecializationRepository.GetUserSpecializationsByExpression(u => u.UserID == request.Email)
                ?? throw new NotFoundException(nameof(User), request.Email);

            var specializations = mapper.Map<SpecializationDto[]>(userSpecializations);

            var userDepartment = await unitOfWork.DepartmentRepository.GetAsync(d => userSpecializations.FirstOrDefault()!.Specialization!.DepartmentID == d.DepartmentID);

            return new()
            {
                DepartmentID = userDepartment!.DepartmentID,
                DepartmentName = userDepartment!.Name,
                Specializations = specializations
            };
        }
    }
}
