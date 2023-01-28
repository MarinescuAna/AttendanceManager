using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Infrastructure.Shared.Logger;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<Course>> GetTeacherCoursesByEmailAsync(string email)
            => await dbContext.Courses.Include(s => s.UserSpecialization).Include(s => s.UserSpecialization!.Specialization)
            .Where(c => c.UserSpecialization!.UserID == email).ToListAsync();
        public async Task<bool> SoftOrHardDelete(int courseId)
        {
            try
            {
                var course = await dbContext.Courses.Include(c => c.Documents).FirstOrDefaultAsync(c => c.CourseID == courseId && !c.IsDeleted);

                if (course == null)
                {
                    return false;
                }

                if (course.Documents?.Count > 0)
                {
                    course.IsDeleted = true;
                    course.UpdatedOn = DateTime.UtcNow;
                    dbContext.Courses.Update(course);
                }
                else
                {
                    course.Documents = null;
                    dbContext.Courses.Remove(course);
                }

                return true;
            }
            catch (Exception ex)
            {
                LoggerSerivce.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
                return false;

            }
        }
    }
}
