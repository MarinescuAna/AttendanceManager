using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManager.Persistance.Repositories
{
    public class SpecialisationRepository : BaseRepository<Specialisation>, ISpecialisationRepository
    {
        public SpecialisationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<Specialisation> GetByIdAsync(Guid id)
            => dbContext.Specialisations
                    .Include(s => s.Department)
                    .FirstOrDefaultAsync(s => s.SpecialisationID == id);
        public override Task<List<Specialisation>> ListAllAsync()
            => dbContext.Specialisations
                .Include(s => s.Department)
                .ToListAsync();
    }
}
