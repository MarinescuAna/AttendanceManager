using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace AttendanceManager.Persistance.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public override Task<Department> GetAsync(Expression<Func<Department, bool>> expression, bool includeNavigationProperty = true)
            => includeNavigationProperty? 
                    dbContext.Departments.Include(s => s.Specializations).AsNoTracking().FirstOrDefaultAsync(expression):
                    dbContext.Departments.AsNoTracking().FirstOrDefaultAsync(expression);
        public override Task<List<Department>> ListAllAsync(bool includeNavigationProperty = true)
            => includeNavigationProperty ?
                    dbContext.Departments.Include(s => s.Specializations).AsNoTracking().ToListAsync():
                    dbContext.Departments.AsNoTracking().ToListAsync();
    }
}
