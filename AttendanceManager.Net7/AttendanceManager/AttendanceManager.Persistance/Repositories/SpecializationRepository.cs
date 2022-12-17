using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AttendanceManager.Persistance.Repositories
{
    public sealed class SpecializationRepository : BaseRepository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
        public override Task<Specialization> GetAsync(Expression<Func<Specialization, bool>> expression, bool includeNavigationProperty = true)
            => includeNavigationProperty ?
                    dbContext.Specializations.Include(s => s.Department).AsNoTracking().FirstOrDefaultAsync(expression) :
                    dbContext.Specializations.AsNoTracking().FirstOrDefaultAsync(expression);
        public override Task<List<Specialization>> ListAllAsync(bool includeNavigationProperty = true)
            => includeNavigationProperty ?
                    dbContext.Specializations.Include(s => s.Department).AsNoTracking().ToListAsync() :
                    dbContext.Specializations.AsNoTracking().ToListAsync();
    }
}
