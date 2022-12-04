using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Persistance.Repositories
{
    public class SpecializationRepository : BaseRepository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
