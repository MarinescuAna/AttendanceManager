using System.Linq.Expressions;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> ListAllAsync();
        void AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
