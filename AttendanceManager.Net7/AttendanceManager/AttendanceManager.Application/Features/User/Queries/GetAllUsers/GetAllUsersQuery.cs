using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetAllUsers
{
    public sealed class GetAllUsersQuery : IRequest<List<UserVm>>
    {
    }
    public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserVm>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var departments = _unitOfWork.DepartmentRepository.ListAll();
            var userSpecializations = await _unitOfWork.UserSpecializationRepository.GetUserSpecializationsByExpression(
                us => us.User!.Role != Role.Admin);

            return userSpecializations.GroupBy(us => us.UserID).Select(user =>
            {
                // get the department
                var department = departments.FirstOrDefault(d => d.DepartmentID == user.FirstOrDefault()!.Specialization!.DepartmentID);
                var userData = user.FirstOrDefault()!.User;

                // create the userDto
                return new UserVm
                {
                    AccountConfirmed = userData!.AccountConfirmed,
                    SpecializationIds = user.Select(us => us.SpecializationID).ToArray(),
                    Code = userData.Code,
                    Created = userData.CreatedOn.ToString(Constants.DateFormat),
                    DepartmentId = department!.DepartmentID,
                    DepartmentName = department!.Name,
                    Email = userData.Email,
                    Year = userData.EnrollmentYear,
                    Fullname = userData.FullName,
                    Role = userData.Role.ToString(),
                    Updated = userData.UpdatedOn.ToString(Constants.DateFormat)
                };

            }).ToList();
        }
    }
}
