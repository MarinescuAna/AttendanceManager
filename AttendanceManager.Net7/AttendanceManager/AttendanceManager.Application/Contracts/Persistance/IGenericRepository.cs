using AttendanceManager.Domain.Enums;
using System.Linq.Expressions;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> expression, NavigationPropertiesSetting setting = NavigationPropertiesSetting.None);
        Task<List<T>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None);
        void AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
