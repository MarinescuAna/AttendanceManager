using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
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
                NavigationPropertiesSetting.OnlyCollectionNavigationProps => dbContext.Users.Include(s => s.UserSpecializations).Include(s => s.Courses).Include(d => d.Documents).Include(ud => ud.UserDocuments).AsNoTracking().FirstOrDefaultAsync(expression),
                _ => dbContext.Users.AsNoTracking().FirstOrDefaultAsync(expression)
            };
        public override Task<List<User>> ListAllAsync(NavigationPropertiesSetting setting = NavigationPropertiesSetting.None)
            => setting switch
            {
                NavigationPropertiesSetting.OnlyCollectionNavigationProps => dbContext.Users.Include(s => s.UserSpecializations).Include(d => d.Documents).Include(ud => ud.UserDocuments).Include(s => s.Courses).AsNoTracking().ToListAsync(),
                _ => dbContext.Users.AsNoTracking().ToListAsync()
            };
    }
}
