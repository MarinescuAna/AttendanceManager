using AttendanceManager.Application.Contracts.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceManager.Persistance.Repositories
{
    public class BaseRepository<T>:IBaseRepository<T> where T : class
    {
        protected readonly AttendanceManagerDbContext dbContext;

        public BaseRepository(AttendanceManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }
        public virtual async Task<List<T>> ListAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return entity;
        }
        public async Task UpdateAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
