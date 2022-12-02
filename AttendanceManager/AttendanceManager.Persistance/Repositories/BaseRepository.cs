using AttendanceManager.Application.Contracts.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AttendanceManager.Persistance.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AttendanceManagerDbContext dbContext;

        public BaseRepository(AttendanceManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await dbContext.Set<T>().FirstOrDefaultAsync(expression);
        }
        public virtual async Task<List<T>> ListAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }
        public async Task<bool> AddAsync(T entity)
        {
            try
            {
                await dbContext.Set<T>().AddAsync(entity);
                return await CommitAsync();
            }
            catch (Exception ex)
            {
                //TODO log when something occures
                return false;
            }
        }
        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                dbContext.Entry(entity).State = EntityState.Modified;
                return await CommitAsync();
            }
            catch (Exception ex)
            {
                //TODO log when something occures
                return false;
            }
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                dbContext.Set<T>().Remove(entity);
                return await CommitAsync();

            }
            catch (Exception ex)
            {
                //TODO log when something occures
                return false;
            }
        }

        private async Task<bool> CommitAsync() =>
            (await dbContext.SaveChangesAsync()) > 0;
    }
}
