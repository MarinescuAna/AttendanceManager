using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Attendance?> GetAsync(Expression<Func<Attendance, bool>> expression)
            => await dbContext.Attendances.Include(a => a.AttendanceCollection).AsNoTracking().FirstOrDefaultAsync(expression);
            
        
        public async Task<bool> HasAttendanceAsync(Expression<Func<Attendance, bool>> expression)
            => await dbContext.Attendances.AnyAsync(expression);

    }
}
