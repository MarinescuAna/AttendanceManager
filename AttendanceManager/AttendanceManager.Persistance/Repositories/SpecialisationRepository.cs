using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AttendanceManager.Persistance.Repositories
{
    public class SpecialisationRepository : BaseRepository<Specialisation>, ISpecialisationRepository
    {
        public SpecialisationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

        public override Task<Specialisation> GetAsync(Expression<Func<Specialisation, bool>> expression)
            => dbContext.Specialisations
                    .Include(s => s.Department)
                    .FirstOrDefaultAsync(expression);
        public override Task<List<Specialisation>> ListAllAsync()
            => dbContext.Specialisations
                .Include(s => s.Department)
                .ToListAsync();
    }
}
