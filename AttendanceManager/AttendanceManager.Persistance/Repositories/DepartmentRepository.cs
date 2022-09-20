using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Persistance.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
