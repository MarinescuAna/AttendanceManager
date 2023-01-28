using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance.Repositories
{
    public sealed class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<List<Specialization>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => dbContext.Specializations.Include(s => s.Department).Include(s => s.UserSpecializations).AsNoTracking().Where(s => !s.IsDeleted).ToListAsync();
    }
}
