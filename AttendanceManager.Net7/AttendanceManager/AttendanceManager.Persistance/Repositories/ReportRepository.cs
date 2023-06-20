using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext,loggingService)
        {
        }

        /// <summary>
        /// The result will contain the Specialization, Course and UserSpecialization
        /// </summary>
        public async Task<Report?> GetReportByIdAsync(int id)
            => await dbContext.Reports
            .Include(d=>d.Collections)
            .Include(d=>d.Course!.UserSpecialization!.Specialization)
            .Include(d=>d.Course!.UserSpecialization!.User)
            .FirstOrDefaultAsync(u => u.ReportID == id);

        /// <summary>
        /// The list will contain the Specialization, Course and UserSpecialization
        /// </summary>
        public async Task<List<Report>> GetUserReportsByExpressionAsync(Expression<Func<Report, bool>> expression)
            => await dbContext.Reports
            .Include(d => d.Course!.UserSpecialization!.Specialization)
            .Include(m=>m.Members)
            .AsNoTracking().Where(expression).ToListAsync();
    }
}
