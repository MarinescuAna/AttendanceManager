using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Persistance.Repositories
{
    public class AttendanceCodeRepository : GenericRepository<AttendanceCode>, IAttendanceCodeRepository
    {
        public AttendanceCodeRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
