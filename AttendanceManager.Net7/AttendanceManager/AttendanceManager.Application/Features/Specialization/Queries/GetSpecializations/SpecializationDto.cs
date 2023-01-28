using AttendanceManager.Application.SharedDtos;

namespace AttendanceManager.Application.Features.Specialization.Queries.GetSpecializations
{
    public sealed class SpecializationDto : BaseDto
    {
        public required int DepartmentId { get; init; }
    }
}
