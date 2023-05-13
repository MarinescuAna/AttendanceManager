using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Persistance.Repositories
{
    public class InvolvementCodeRepository : GenericRepository<InvolvementCode>, IInvolvementCodeRepository
    {
        public InvolvementCodeRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
