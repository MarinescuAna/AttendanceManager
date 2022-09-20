using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
