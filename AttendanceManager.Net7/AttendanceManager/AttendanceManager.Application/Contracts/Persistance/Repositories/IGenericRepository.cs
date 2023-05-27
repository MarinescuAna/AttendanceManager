using System.Linq.Expressions;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> ListAll();
        void AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
