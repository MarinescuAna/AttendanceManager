using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<Course?> GetAsync(Expression<Func<Course, bool>> expression, NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.None => dbContext.Courses.FirstOrDefaultAsync(expression),
                _ => dbContext.Courses.Include(s => s.Specialization).FirstOrDefaultAsync(expression)
            };

        public override Task<List<Course>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.None => dbContext.Courses.AsNoTracking().Where(u => !u.IsDeleted).ToListAsync(),
                _ => dbContext.Courses.Include(s => s.Specialization).AsNoTracking().Where(u => !u.IsDeleted).ToListAsync(),
            };
    }
}
