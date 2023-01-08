using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public sealed class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public override Task<Department?> GetAsync(Expression<Func<Department, bool>> expression, NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.None => dbContext.Departments.FirstOrDefaultAsync(expression),
                _ => dbContext.Departments.Include(s => s.Specializations).FirstOrDefaultAsync(expression)
            };
        public override Task<List<Department>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.None => dbContext.Departments.AsNoTracking().Where(u => !u.IsDeleted).ToListAsync(),
                _ => dbContext.Departments.Include(s => s.Specializations).AsNoTracking().Where(u => !u.IsDeleted).ToListAsync()
            };
    }
}
