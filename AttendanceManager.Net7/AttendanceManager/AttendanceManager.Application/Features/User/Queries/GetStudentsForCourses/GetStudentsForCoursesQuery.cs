using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetStudentsForCourses
{
    /// <summary>
    /// Get the list with all the students that have the given enrollment year and specialization id
    /// </summary>
    public sealed class GetStudentsForCoursesQuery : IRequest<List<StudentDto>>
    {
        public required int EnrollmentYear { get; init; }
        public required int SpecializationId { get; init; }

    }
}
