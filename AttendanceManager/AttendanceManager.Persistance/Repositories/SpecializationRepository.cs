using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AttendanceManager.Persistance.Repositories
{
    public class SpecializationRepository : BaseRepository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        //public override Task<Specialization> GetAsync(Expression<Func<Specialization, bool>> expression)
        //    => dbContext.Specializations
        //        .Include(d => d.Department)
        //        .FirstOrDefaultAsync(expression);

        //public override Task<List<Specialization>> ListAllAsync()
        //    => dbContext.Specializations
        //        .Include(d => d.Department)
        //        .ToListAsync();

    }
}
