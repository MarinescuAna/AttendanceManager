using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetStudentsForCourses
{
    public sealed class GetStudentsForCoursesQuery : IRequest<List<StudentDto>>
    {
        public required string EnrollmentYear { get; init; }
        public required string SpecializationId { get; init; }

    }
}
