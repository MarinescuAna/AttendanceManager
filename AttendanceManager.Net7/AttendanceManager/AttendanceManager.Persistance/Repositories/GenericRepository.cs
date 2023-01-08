using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Enums;
using AttendanceManager.Infrastructure.Shared.Logger;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AttendanceManagerDbContext dbContext;

        public GenericRepository(AttendanceManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> expression, NavigationPropertiesSetting setting = NavigationPropertiesSetting.None) =>
            await dbContext.Set<T>().FirstOrDefaultAsync(expression);
        public virtual async Task<List<T>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None) =>
            await dbContext.Set<T>().AsNoTracking().ToListAsync();
        public async void AddAsync(T entity)
        {
            try
            {
                await dbContext.Set<T>().AddAsync(entity);
            }
            catch (Exception ex)
            {
                LoggerSerivce.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
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
                LoggerSerivce.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
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
                LoggerSerivce.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
            }
        }

    }
}
