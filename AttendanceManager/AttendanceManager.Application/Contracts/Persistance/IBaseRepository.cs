using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T,bool>> expression);
        Task<List<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
