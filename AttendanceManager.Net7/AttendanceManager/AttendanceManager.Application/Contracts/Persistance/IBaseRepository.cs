using System.Linq.Expressions;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> expression, bool includeNavigationProperty = true);
        Task<List<T>> ListAllAsync(bool includeNavigationProperty = true);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
