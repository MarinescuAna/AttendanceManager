using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        
        public CourseRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext,loggingService)
        {
        }
        public async Task<List<Course>> GetTeacherCoursesByEmailAsync(string email)
            => await dbContext.Courses
                .Include(s => s.UserSpecialization)
                .Include(s => s.UserSpecialization!.Specialization)
                .Include(s=>s.Documents)
                .Where(c => c.UserSpecialization!.UserID == email)
                .ToListAsync();
        public async Task<List<Course>> GetCoursesForDashboardAsync(string email)
           => await dbContext.Courses
                .Include(s => s.UserSpecialization)
                .Include(s => s.Documents)
                    .ThenInclude(s=>s.Members)
                        .ThenInclude(m=>m.User)
                .Include(s => s.Documents)
                    .ThenInclude(s => s.Collections)
                        .ThenInclude(a => a.Attendances)
                .AsNoTracking()
                .Where(d=>d.UserSpecialization!.UserID.Equals(email))
                .ToListAsync();
            
        public override async Task<Course?> GetAsync(Expression<Func<Course, bool>> expression) =>
          await dbContext.Set<Course>().Include(c=>c.Documents).FirstOrDefaultAsync(expression);
    }
}
