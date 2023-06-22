using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.Repositories
{
    public sealed class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext, loggingService)
        {
        }

        public override IQueryable<Department> ListAll() => dbContext.Departments.Include(a => a.Specializations).AsNoTracking();
    }
}
