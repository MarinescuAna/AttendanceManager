using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class InvolvementRepository : GenericRepository<Involvement>, IInvolvementRepository
    {
        public InvolvementRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext,loggingService)
        {
        }
        public async Task AddRangeAsync(List<Involvement> entity)
        {
            try
            {
                await dbContext.Involvements.AddRangeAsync(entity);
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
            }
        }
        public override IQueryable<Involvement> ListAll() => dbContext.Involvements.Include(a => a.Collection).AsNoTracking();
        public override async Task<Involvement?> GetAsync(Expression<Func<Involvement, bool>> expression)
            => await dbContext.Involvements.Include(a => a.Collection).AsNoTracking().FirstOrDefaultAsync(expression);
        public IEnumerable<Involvement> GetInvolvementsByReportId(int id)
            => dbContext.Involvements.Include(a => a.Collection)
                .AsNoTracking().Where(a => a.Collection!.ReportID == id).AsEnumerable();
    }
}
