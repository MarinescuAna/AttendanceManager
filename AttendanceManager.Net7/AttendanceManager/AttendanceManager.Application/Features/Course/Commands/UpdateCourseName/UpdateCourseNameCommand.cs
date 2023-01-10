using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.UpdateCourseName
{
    public sealed class UpdateCourseNameCommand:IRequest<bool>
    {
        public required string CourseId { get; init; }
        public required string Name { get; init; }
    }
}
