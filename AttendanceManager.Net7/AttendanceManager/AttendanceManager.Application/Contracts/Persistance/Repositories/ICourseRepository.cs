using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<List<Course>> GetTeacherCoursesByEmailAsync(string email);
        Task<List<Course>> GetCoursesForDashboardAsync(string email);
    }
}
