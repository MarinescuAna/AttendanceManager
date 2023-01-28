using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public sealed class UserSpecializationRepository : GenericRepository<UserSpecialization>, IUserSpecializationRepository
    {
        public UserSpecializationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<UserSpecialization>> GetUserSpecializationsByExpression(Expression<Func<UserSpecialization, bool>> expression)
            => await dbContext.UserSpecializations.Include(u => u.User).Include(s => s.Specialization).AsNoTracking().Where(expression).ToListAsync();
    }
}
