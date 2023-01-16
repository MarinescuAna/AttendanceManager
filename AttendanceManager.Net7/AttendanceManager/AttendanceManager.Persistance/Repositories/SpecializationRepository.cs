using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public sealed class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public override Task<Specialization?> GetAsync(Expression<Func<Specialization, bool>> expression, NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.None => dbContext.Specializations.FirstOrDefaultAsync(expression),
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.Specializations.Include(s => s.Department).FirstOrDefaultAsync(expression),
                NavigationPropertiesSetting.OnlyCollectionNavigationProps => dbContext.Specializations.Include(d=>d.Documents).Include(s => s.UserSpecializations).Include(c => c.Courses).FirstOrDefaultAsync(expression),
                _ => dbContext.Specializations.Include(s => s.Department).Include(d => d.Documents).Include(s => s.UserSpecializations).Include(c => c.Courses).FirstOrDefaultAsync(expression)
            };
        public override Task<List<Specialization>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.None => dbContext.Specializations.AsNoTracking().ToListAsync(),
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.Specializations.Include(s => s.Department).AsNoTracking().ToListAsync(),
                NavigationPropertiesSetting.OnlyCollectionNavigationProps => dbContext.Specializations.Include(d => d.Documents).Include(s => s.UserSpecializations).Include(c => c.Courses).AsNoTracking().ToListAsync(),
                _ => dbContext.Specializations.Include(s => s.Department).Include(d => d.Documents).Include(s => s.UserSpecializations).Include(c => c.Courses).AsNoTracking().ToListAsync()
            };
    }
}
