using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.UpdateCourseName
{
    /// <summary>
    /// Update only the course name and the UpdateOn field
    /// </summary>
    public sealed class UpdateCourseNameCommand:IRequest<bool>
    {
        public required int CourseId { get; init; }
        public required string Name { get; init; }
    }
}
