using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Core.Shared;
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
            var departments = await unitOfWork.DepartmentRepository.ListAllAsync();
            var userSpecializations = await unitOfWork.UserSpecializationRepository.GetUserSpecializationsByExpression(
                us => us.User!.Role != Role.Admin);

            return userSpecializations.GroupBy(us => us.UserID).Select(user =>
            {
                // get the department
                var department = departments.FirstOrDefault(d => d.DepartmentID==user.FirstOrDefault()!.Specialization!.DepartmentID);
                var userData = user.FirstOrDefault()!.User;

                // create the userDto
                return new UserDto
                {
                    AccountConfirmed = userData!.AccountConfirmed,
                    UserSpecializations = user.Select(us => new SpecializationDto()
                    {
                        Id = us.SpecializationID,
                        Name = us.Specialization!.Name
                    }).ToArray(),
                    Code = userData.Code,
                    Created = userData.CreatedOn.ToString(Constants.DateFormat),
                    DepartmentId = department!.DepartmentID,
                    DepartmentName = department!.Name,
                    Email = userData.Email,
                    EnrollmentYear = userData.EnrollmentYear,
                    Fullname = userData.FullName,
                    Role = userData.Role.ToString(),
                    Updated = userData.UpdatedOn.ToString(Constants.DateFormat)
                };

            }).ToList();
        }
    }
}
