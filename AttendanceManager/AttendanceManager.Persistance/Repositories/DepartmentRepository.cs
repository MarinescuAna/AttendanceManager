using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace AttendanceManager.Persistance.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<Department> GetAsync(Expression<Func<Department, bool>> expression)
            => dbContext.Departments
                    .Include(s => s.Specializations)
                    .FirstOrDefaultAsync(expression);
        public override Task<List<Department>> ListAllAsync()
            => dbContext.Departments
                .Include(s => s.Specializations)
                .ToListAsync();
    }
}
