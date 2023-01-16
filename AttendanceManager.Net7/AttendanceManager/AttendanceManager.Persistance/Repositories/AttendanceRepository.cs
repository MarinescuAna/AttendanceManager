using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<Attendance?> GetAsync(Expression<Func<Attendance, bool>> expression, NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.Attendances.Include(u => u.User).Include(us => us.DocumentFile).FirstOrDefaultAsync(expression),
                _ => dbContext.Attendances.FirstOrDefaultAsync(expression)
            };

        public override Task<List<Attendance>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.Attendances.Include(u => u.User).Include(us => us.DocumentFile).AsNoTracking().ToListAsync(),
                _ => dbContext.Attendances.AsNoTracking().ToListAsync()
            };
    }
}
