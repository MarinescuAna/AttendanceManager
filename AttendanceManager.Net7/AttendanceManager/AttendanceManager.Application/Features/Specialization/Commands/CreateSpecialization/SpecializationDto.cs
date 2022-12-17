namespace AttendanceManager.Application.Features.Specialization.Commands.CreateSpecialization
{
    public sealed class SpecializationDto
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required Guid DepartmentId { get; init; }
    }
}
