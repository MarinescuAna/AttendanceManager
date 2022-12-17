using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{

    public sealed class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public override Task<User> GetAsync(Expression<Func<User, bool>> expression, bool includeNavigationProperty = true)
            => includeNavigationProperty ?
                    dbContext.Users.Include(s => s.UserSpecializations).AsNoTracking().FirstOrDefaultAsync(expression) :
                    dbContext.Users.AsNoTracking().FirstOrDefaultAsync(expression);
        public override Task<List<User>> ListAllAsync(bool includeNavigationProperty = true)
            => includeNavigationProperty ?
                    dbContext.Users.Include(s => s.UserSpecializations).AsNoTracking().ToListAsync() :
                    dbContext.Users.AsNoTracking().ToListAsync();
    }
}
