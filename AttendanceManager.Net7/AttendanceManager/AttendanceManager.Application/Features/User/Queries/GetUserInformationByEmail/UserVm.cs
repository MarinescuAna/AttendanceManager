namespace AttendanceManager.Application.Features.User.Queries.GetUserInformationByEmail
{
    public sealed class UserVm
    {
        public required int DepartmentID { get; init; }
        public required string DepartmentName { get; init; }
        public required SpecializationDto[] Specializations { get; init; }
    }
    public sealed class SpecializationDto
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
    }
}
