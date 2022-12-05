using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AttendanceManager.Persistance.Repositories
{
    public class UserSpecializationRepository : BaseRepository<UserSpecialization>, IUserSpecializationRepository
    {
        public UserSpecializationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public override Task<UserSpecialization> GetAsync(Expression<Func<UserSpecialization, bool>> expression, bool includeNavigationProperty = true)
            => includeNavigationProperty? 
                    dbContext.UserSpecializations.Include(u=>u.User).Include(us=>us.Specialization).AsNoTracking().FirstOrDefaultAsync(expression):
                    dbContext.UserSpecializations.AsNoTracking().FirstOrDefaultAsync(expression);

        public override Task<List<UserSpecialization>> ListAllAsync(bool includeNavigationProperty = true)
            => includeNavigationProperty? 
            dbContext.UserSpecializations.Include(u => u.User).Include(us => us.Specialization).AsNoTracking().ToListAsync():
            dbContext.UserSpecializations.AsNoTracking().ToListAsync();
    }
}
