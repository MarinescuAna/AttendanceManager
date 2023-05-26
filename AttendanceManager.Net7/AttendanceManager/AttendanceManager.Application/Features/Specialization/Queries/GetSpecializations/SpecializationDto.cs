namespace AttendanceManager.Application.Features.Specialization.Queries.GetSpecializations
{
    public sealed class SpecializationDto
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required int DepartmentId { get; init; }

    }
}
