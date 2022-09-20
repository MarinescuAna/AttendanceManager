using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManager.Persistance.Repositories
{
    public class UserSpecialisationRepository : BaseRepository<UserSpecialisation>, IUserSpecialisationRepository
    {
        public UserSpecialisationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<UserSpecialisation> GetByIdAsync(Guid id)
            => dbContext.UserSpecialisations
                .Include(us=>us.User)
                .Include(us=>us.Specialisation)
                .FirstOrDefaultAsync(us=>us.UserSpecialisationID == id);

        public override Task<List<UserSpecialisation>> ListAllAsync()
            => dbContext.UserSpecialisations
                .Include(us => us.User)
                .Include(us => us.Specialisation)
                .ToListAsync();
            
        

    }
}
