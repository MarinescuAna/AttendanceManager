using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Persistance.Repositories
{
    public sealed class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
