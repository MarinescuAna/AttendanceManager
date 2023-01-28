using AttendanceManager.Application.SharedDtos;

namespace AttendanceManager.Application.Features.User.Queries.GetUserInformationByEmail
{
    public sealed class UserInfoDto
    {
        public required int DepartmentID { get; init; }
        public required string DepartmentName { get; init; }
        public required SpecializationDto[] Specializations { get; init; }
    }
}
