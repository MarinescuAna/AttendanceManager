using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public sealed class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public override Task<Department> GetAsync(Expression<Func<Department, bool>> expression, bool includeNavigationProperty = true)
            => includeNavigationProperty ?
                    dbContext.Departments.Include(s => s.Specializations).FirstOrDefaultAsync(expression) :
                    dbContext.Departments.FirstOrDefaultAsync(expression);
        public override Task<List<Department>> ListAllAsync(bool includeNavigationProperty = true)
            => includeNavigationProperty ?
                    dbContext.Departments.Include(s => s.Specializations).AsNoTracking().Where(u=>!u.IsDeleted).ToListAsync() :
                    dbContext.Departments.AsNoTracking().Where(u => !u.IsDeleted).ToListAsync();
    }
}
