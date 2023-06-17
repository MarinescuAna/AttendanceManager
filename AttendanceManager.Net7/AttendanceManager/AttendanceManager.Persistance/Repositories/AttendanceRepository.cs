using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext,loggingService)
        {
        }

        public override IQueryable<Attendance> ListAll() => dbContext.Attendances.Include(a => a.Collection).AsNoTracking();
        public override async Task<Attendance?> GetAsync(Expression<Func<Attendance, bool>> expression)
            => await dbContext.Attendances.Include(a => a.Collection).AsNoTracking().FirstOrDefaultAsync(expression);
        public IEnumerable<Attendance> GetAttendancesByReportId(int id)
            => dbContext.Attendances.Include(a => a.Collection)
                .AsNoTracking().Where(a => a.Collection!.DocumentID == id).AsEnumerable();
    }
}
