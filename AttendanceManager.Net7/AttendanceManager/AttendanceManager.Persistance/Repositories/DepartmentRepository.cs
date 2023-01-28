using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using AttendanceManager.Infrastructure.Shared.Logger;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.Repositories
{
    public sealed class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<List<Department>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => dbContext.Departments.AsNoTracking().Where(u => !u.IsDeleted).ToListAsync();

        public async Task<bool> SoftOrHardDelete(int id)
        {
            try
            {
                var department = await dbContext.Departments.Include(d => d.Specializations).AsNoTracking()
                    .FirstOrDefaultAsync(d => d.DepartmentID == id && !d.IsDeleted);

                if (department == null)
                {
                    return false;
                }

                if (department!.Specializations?.Count() > 0)
                {
                    department.IsDeleted = true;
                    department.UpdatedOn = DateTime.UtcNow;
                    dbContext.Update(department);
                }
                else
                {
                    dbContext.Remove(department);
                }

                return true;
            }
            catch (Exception ex)
            {
                LoggerSerivce.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
                return false;
            }
        }
    }
}
