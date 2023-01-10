using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.CreateCourse
{
    public sealed class CreateCourseCommand : IRequest<Guid>
    {
        public required string Name { get; init; }
        public required string SpecializationId { get; init; }
        public string? Email { get; set; }
    }
}
