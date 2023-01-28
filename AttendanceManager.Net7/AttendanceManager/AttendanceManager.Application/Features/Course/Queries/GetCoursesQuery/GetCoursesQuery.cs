using MediatR;

namespace AttendanceManager.Application.Features.Course.Queries.GetCoursesQuery
{
    /// <summary>
    /// Get all the courses own by the current user, which is only the teachers, not admin or students
    /// </summary>
    public sealed class GetCoursesQuery : IRequest<List<CoursesDto>>
    {
        public required string Email { get; init; }
    }
}
