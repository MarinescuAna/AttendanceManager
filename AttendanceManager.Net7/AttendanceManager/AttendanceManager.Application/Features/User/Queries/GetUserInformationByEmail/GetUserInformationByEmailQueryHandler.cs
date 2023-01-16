using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.SharedDtos;
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
            var user = await unitOfWork.UserRepository.GetAsync(u => u.Email == request.Email, Domain.Enums.NavigationPropertiesSetting.OnlyCollectionNavigationProps)
                ?? throw new NotFoundException(nameof(User), request.Email);

            if (user.UserSpecializations != null)
            {
                var userSpecializationIds = user.UserSpecializations.Select(u=>u.SpecializationID).ToArray();
                var userSpecializations = (await unitOfWork.SpecializationRepository.ListAllAsync())
                    .Where(s=>userSpecializationIds.Any(u=>u==s.SpecializationID))
                    .Select(s=>new SpecializationDto()
                    {
                        Id = s.SpecializationID.ToString(),
                        Name = s.Name
                    })
                    .ToArray();

                var userDepartment = await unitOfWork.SpecializationRepository.GetAsync(s => s.SpecializationID == userSpecializationIds.FirstOrDefault() && !s.IsDeleted, Domain.Enums.NavigationPropertiesSetting.OnlyReferenceNavigationProps);

                return new()
                {
                    DepartmentID = userDepartment!.DepartmentID.ToString(),
                    DepartmentName = userDepartment!.Name,
                    Specializations = userSpecializations
                };
            }

            return null;
        }
    }
}
