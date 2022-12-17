using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Application.Shared;
using AttendanceManager.Application.SharedDtos;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceManager.Application.Features.User.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : UserFeatureBase, IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly ISpecializationRepository _specializationRepository;
        public GetAllUsersQueryHandler(IUserRepository userRepo, ISpecializationRepository specializationRepository, IMapper map) : base(userRepo, map)
        {
            _specializationRepository = specializationRepository;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            // get all the users and all the specializations
            var users = await userRepository.ListAllAsync();
            var specializations = await _specializationRepository.ListAllAsync();

            return users.Select(user =>
            {
                // get the department
                var department = specializations.FirstOrDefault(s => s.SpecializationID == user.UserSpecializations.FirstOrDefault()?.SpecializationID)?.Department;

                // create the userDto
                return new UserDto
                {
                    UserId = user.UserID.ToString(),
                    AccountConfirmed = user.AccountConfirmed,
                    UserSpecializations = user.UserSpecializations.Select(us => new SpecializationDto()
                    {
                        Id = us.SpecializationID,
                        Name = specializations.FirstOrDefault(s => s.SpecializationID == us.SpecializationID)?.Name
                    }).ToArray(),
                    Code = user.Code,
                    Created = user.Created.ToString(Constants.DateFormat),
                    DepartmentId = department?.DepartmentID.ToString(),
                    DepartmentName = department?.Name,
                    Email = user.Email,
                    EnrollmentYear = user.EnrollmentYear,
                    Fullname = user.FullName,
                    Role = user.Role.ToString(),
                    Updated = user.Updated.ToString(Constants.DateFormat)
                };

            }).ToList();
        }
    }
}
