using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AttendanceManager.Persistance.Repositories
{
    public class UserSpecialisationRepository : BaseRepository<UserSpecialisation>, IUserSpecialisationRepository
    {
        public UserSpecialisationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<UserSpecialisation> GetAsync(Expression<Func<UserSpecialisation, bool>> expression)
            => dbContext.UserSpecialisations
                .Include(us=>us.User)
                .Include(us=>us.Specialisation)
                .FirstOrDefaultAsync(expression);

        public override Task<List<UserSpecialisation>> ListAllAsync()
            => dbContext.UserSpecialisations
                .Include(us => us.User)
                .Include(us => us.Specialisation)
                .ToListAsync();
            
        

    }
}
