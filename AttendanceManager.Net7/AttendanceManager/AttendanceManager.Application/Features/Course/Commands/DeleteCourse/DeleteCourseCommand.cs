using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.DeleteCourse
{
    public sealed class DeleteCourseCommand : IRequest<bool>
    {
        public required string Id { get; init; }
    }
}
