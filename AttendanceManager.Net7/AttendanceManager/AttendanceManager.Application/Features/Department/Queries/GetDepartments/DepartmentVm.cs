namespace AttendanceManager.Application.Features.Department.Queries.GetDepartments
{
    public class DepartmentVm
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string UpdatedOn { get; init; }
        public required int LinkedSpecializations { get; init; }
    }
}
