using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AttendanceManagerDbContext dbContext;
        protected readonly ILoggingService loggingService;
        public GenericRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService)
        {
            this.loggingService= loggingService;    
            this.dbContext = dbContext;
        }
        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> expression) =>
            await dbContext.Set<T>().FirstOrDefaultAsync(expression);
        public virtual IQueryable<T> ListAll() => dbContext.Set<T>().AsNoTracking();
        public async Task AddAsync(T entity)
        {
            try
            {
                await dbContext.Set<T>().AddAsync(entity);
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
            }
        }
        public void Update(T entity)
        {
            try
            {
                dbContext.Set<T>().Update(entity);
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                dbContext.Set<T>().Remove(entity);
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
            }
        }

    }
}
