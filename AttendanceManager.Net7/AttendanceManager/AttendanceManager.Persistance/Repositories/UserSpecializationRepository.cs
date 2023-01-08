using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
                NavigationPropertiesSetting.None =>  dbContext.UserSpecializations.FirstOrDefaultAsync(expression),
                _ =>dbContext.UserSpecializations.Include(u => u.User).Include(us => us.Specialization).FirstOrDefaultAsync(expression)
            };

        public override Task<List<UserSpecialization>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.OnlyReferenceNavigationProps => dbContext.UserSpecializations.AsNoTracking().ToListAsync(),
                _ => dbContext.UserSpecializations.Include(u => u.User).Include(us => us.Specialization).AsNoTracking().ToListAsync()
            };
    }
}
