namespace AttendanceManager.Application.Features.User.Queries.GetStudentsForCourses
{
    public sealed class StudentDto
    {
        public required string Fullname { get; init; }
        public required string Email { get; init; }
    }
}