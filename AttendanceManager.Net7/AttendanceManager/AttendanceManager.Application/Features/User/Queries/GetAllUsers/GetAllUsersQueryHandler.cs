using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Shared;
using AttendanceManager.Application.SharedDtos;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetAllUsers
{
    public sealed class GetAllUsersQueryHandler : BaseFeature, IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper map) : base(unitOfWork, map)
        {
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            // get all the users and all the specializations
            var users = await unitOfWork.UserRepository.ListAllAsync(NavigationPropertiesSetting.OnlyCollectionNavigationProps);
            var specializations = await unitOfWork.SpecializationRepository.ListAllAsync(NavigationPropertiesSetting.OnlyReferenceNavigationProps);

            return users.Where(u => u.Role != Role.Admin).Select(user =>
            {
                // get the department
                var department = specializations.FirstOrDefault(s => s.SpecializationID == user.UserSpecializations?.FirstOrDefault()?.SpecializationID)?.Department;

                // create the userDto
                return new UserDto
                {
                    UserId = user.UserID.ToString(),
                    AccountConfirmed = user.AccountConfirmed,
                    UserSpecializations = user?.UserSpecializations?.Select(us => new SpecializationDto()
                    {
                        Id = us.SpecializationID,
                        Name = specializations.FirstOrDefault(s => s.SpecializationID == us.SpecializationID)?.Name
                    })?.ToArray(),
                    Code = user?.Code,
                    Created = user.Created.ToString(Constants.DateFormat),
                    DepartmentId = department?.DepartmentID.ToString(),
                    DepartmentName = department?.Name,
                    Email = user.Email,
                    EnrollmentYear = (int)user?.EnrollmentYear,
                    Fullname = user.FullName,
                    Role = user.Role.ToString(),
                    Updated = user.Updated.ToString(Constants.DateFormat)
                };

            }).ToList();
        }
    }
}
