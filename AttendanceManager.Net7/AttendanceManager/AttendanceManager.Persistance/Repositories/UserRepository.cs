using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{

    public sealed class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public override Task<User?> GetAsync(Expression<Func<User, bool>> expression, NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.OnlyCollectionNavigationProps => dbContext.Users.Include(s => s.UserSpecializations).Include(s => s.Attendances).Include(d => d.DocumentMembers).AsNoTracking().FirstOrDefaultAsync(expression),
                _ => dbContext.Users.AsNoTracking().FirstOrDefaultAsync(expression)
            };
        public override Task<List<User>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.OnlyCollectionNavigationProps => dbContext.Users.Include(d => d.Attendances).Include(s => s.UserSpecializations).Include(d => d.DocumentMembers).AsNoTracking().Where(u => !u.IsDeleted).ToListAsync(),
                _ => dbContext.Users.AsNoTracking().ToListAsync()
            };

    }
}
