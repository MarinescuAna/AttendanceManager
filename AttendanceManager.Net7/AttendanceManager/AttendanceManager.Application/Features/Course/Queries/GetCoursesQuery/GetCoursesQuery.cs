using MediatR;

namespace AttendanceManager.Application.Features.Course.Queries.GetCoursesQuery
{
    public sealed class GetCoursesQuery : IRequest<List<CoursesDto>>
    {
        public required string Email { get; init; }
    }
}
