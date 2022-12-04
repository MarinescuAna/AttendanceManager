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

        public override Task<UserSpecialization> GetAsync(Expression<Func<UserSpecialization, bool>> expression)
            => dbContext.UserSpecializations
                .Include(u=>u.User)
                .Include(us=>us.Specialization)
                .FirstOrDefaultAsync(expression);

        public override Task<List<UserSpecialization>> ListAllAsync()
            => dbContext.UserSpecializations
                .Include(u => u.User)
                .Include(us => us.Specialization)
                .ToListAsync();
    }
}
