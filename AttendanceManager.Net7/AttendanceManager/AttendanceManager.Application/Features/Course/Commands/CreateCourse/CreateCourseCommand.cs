using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.CreateCourse
{
    /// <summary>
    /// A course can be created only by the teacher
    /// 
    /// Params:
    /// course name
    /// userSpecializationId
    /// 
    /// Return:
    /// The id of the new course
    /// </summary>
    public sealed class CreateCourseCommand : IRequest<int>
    {
        public required string Name { get; init; }
        public required int UserSpecializationId { get; init; }
    }
}
