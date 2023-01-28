using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public sealed class UserSpecializationRepository : GenericRepository<UserSpecialization>, IUserSpecializationRepository
    {
        public UserSpecializationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public override Task<UserSpecialization?> GetAsync(Expression<Func<UserSpecialization, bool>> expression, NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.None => dbContext.UserSpecializations.FirstOrDefaultAsync(expression),
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.UserSpecializations.Include(u => u.User).Include(us => us.Specialization).FirstOrDefaultAsync(expression),
                NavigationPropertiesSetting.OnlyCollectionNavigationProps => dbContext.UserSpecializations.Include(u => u.Courses).FirstOrDefaultAsync(expression),
                _ => dbContext.UserSpecializations.Include(u => u.User).Include(c => c.Courses).Include(s => s.Specialization).FirstOrDefaultAsync(expression)
            };

        public override Task<List<UserSpecialization>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.None => dbContext.UserSpecializations.AsNoTracking().ToListAsync(),
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.UserSpecializations.Include(u => u.User).Include(us => us.Specialization).AsNoTracking().ToListAsync(),
                NavigationPropertiesSetting.OnlyCollectionNavigationProps => dbContext.UserSpecializations.Include(u => u.Courses).AsNoTracking().ToListAsync(),
                _ => dbContext.UserSpecializations.Include(u => u.User).Include(c => c.Courses).Include(s => s.Specialization).AsNoTracking().ToListAsync()
            };

        public async Task<List<UserSpecialization>> GetUserSpecializationsByExpression(Expression<Func<UserSpecialization,bool>> expression)
            => await dbContext.UserSpecializations.Include(u => u.User).Include(s => s.Specialization).AsNoTracking().Where(expression).ToListAsync();
    }
}
