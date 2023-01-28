using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.DeleteCourse
{
    /// <summary>
    /// This is a soft or hard delete, depending on the collection navigation properties:
    /// if there are some data in any collection
    /// 
    /// </summary>
    public sealed class DeleteCourseCommand : IRequest<bool>
    {
        public required int Id { get; init; }
    }
}
